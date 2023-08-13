using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using RazerSdkReader.Extensions;
using RazerSdkReader.Structures;

namespace RazerSdkReader;

[SupportedOSPlatform("windows")]
public sealed class RazerSdkReader : IDisposable
{
    private readonly Queue<Mutex> _mutexes = new();
    private TaskCompletionSource? _initCompletionSource;
    private AutoResetEvent? _disposeEvent;
    private Thread? _mutexThread;

    private SignaledMemoryReader<CChromaKeyboard>? _keyboardReader;
    private SignaledMemoryReader<CChromaMouse>? _mouseReader;
    private SignaledMemoryReader<CChromaMousepad>? _mousepadReader;
    private SignaledMemoryReader<CChromaKeypad>? _keypadReader;
    private SignaledMemoryReader<CChromaHeadset>? _headsetReader;
    private SignaledMemoryReader<CChromaLink>? _chromaLinkReader;
    private SignaledMemoryReader<CAppData>? _appDataReader;
    private SignaledMemoryReader<CAppManager>? _appManagerReader;

    public event EventHandler<CChromaKeyboard>? KeyboardUpdated;
    public event EventHandler<CChromaMouse>? MouseUpdated;
    public event EventHandler<CChromaMousepad>? MousepadUpdated;
    public event EventHandler<CChromaKeypad>? KeypadUpdated;
    public event EventHandler<CChromaHeadset>? HeadsetUpdated;
    public event EventHandler<CChromaLink>? ChromaLinkUpdated;
    public event EventHandler<CAppData>? AppDataUpdated;
    public event EventHandler<CAppManager>? AppManagerUpdated;

    private CancellationTokenSource _cancellationToken = new();

    public void Start()
    {
        if (_mutexThread is { IsAlive: true })
            return;
        
        var isServiceRunning = Process.GetProcessesByName("RzSDKService").Any();
        
        if (!isServiceRunning)
            throw new InvalidOperationException("RzSdkService is not running.");
        
        _disposeEvent = new(false);
        _initCompletionSource = new();
        _mutexThread = new Thread(Thread);
        _mutexThread.Start();
        
        _initCompletionSource.Task.Wait();
    }

    private void Thread()
    {
        _cancellationToken = new();
        try
        {
            InitMutexes();
            InitAppReaders();
        }
        catch (Exception ex)
        {
            _initCompletionSource?.TrySetException(new InvalidOperationException("Failed to initialize RazerSdkReader.", ex));
            return;
        }

        _initCompletionSource?.TrySetResult();
        FirstThreadLoop();
        if (!_cancellationToken.IsCancellationRequested)
        {
            ThreadLoop();
        }

        _disposeEvent?.WaitOne();

        try
        {
            DisposeReaders();
            DisposeMutexes();
        }
        catch
        {
            //dispose errors are not important
        }
    }

    private void FirstThreadLoop()
    {
        WaitHandle[] handles = { _cancellationToken.Token.WaitHandle, _appDataReader!.EventWaitHandle };

        bool loop;
        do
        {
            var providerIndex = WaitHandle.WaitAny(handles);

            switch (providerIndex)
            {
                case 0: // cancelled
                    return;
                default:
                    var provider = _appDataReader;
                    provider.Update();
                    var app = "app"; //TODO update with new app name

                    if (string.IsNullOrEmpty(app))
                    {
                        loop = true;
                        break;
                    }

                    InitReaders();
                    return;
            }
        } while (loop);
    }

    private void ThreadLoop()
    {
        var readers = new[]
        {
            (SignaledReader)null!, _keyboardReader, _mouseReader, _mousepadReader, _keypadReader, _headsetReader, _chromaLinkReader,
            _appDataReader, _appManagerReader
        };
        var readerHandles = readers
            .Where(r => r != null)
            .Select(r => r!.EventWaitHandle)
            .ToArray();

        SignaledReader[] theReaders = readers.Where(r => r != null).ToArray();
        WaitHandle[] handles = new[] { _cancellationToken.Token.WaitHandle }.Concat(readerHandles).ToArray();

        var loop = true;
        while (loop)
        {
            var providerIndex = WaitHandle.WaitAny(handles);

            switch (providerIndex)
            {
                case 0: // cancelled
                    loop = false;
                    break;
                default:
                    var provider = theReaders[providerIndex - 1];
                    try
                    {
                        provider.Update();  //TODO full update shouldn't be done here, just mark dirty
                    }
                    catch
                    { /* ignore */ }
                    break;
            }
        }
    }

    private void InitMutexes()
    {
        _mutexes.Enqueue(new Mutex(true, Constants.SynapseOnlineMutex));
        EventWaitHandleHelper.Pulse(Constants.SynapseEvent);
        _mutexes.Enqueue(new Mutex(true, Constants.OldSynapseOnlineMutex));
        EventWaitHandleHelper.Pulse(Constants.SynapseEvent);
        _mutexes.Enqueue(new Mutex(true, Constants.OldSynapseVersionMutex));
        EventWaitHandleHelper.Pulse(Constants.SynapseEvent);
        _mutexes.Enqueue(new Mutex(true, Constants.ChromaEmulatorMutex));
    }
    
    private void DisposeMutexes()
    {
        while (_mutexes.TryDequeue(out var mutex))
        {
            mutex.ReleaseMutex();
            mutex.Dispose();
        }
    }

    private void InitAppReaders()
    {
        _appDataReader = new SignaledMemoryReader<CAppData>(Constants.CAppDataFileMapping, Constants.CAppDataEvent);
        _appDataReader.Updated += OnAppDataReaderOnUpdated;
        
        _appManagerReader = new SignaledMemoryReader<CAppManager>(Constants.CAppManagerFileMapping, Constants.CAppManagerEvent);
        _appManagerReader.Updated += OnAppManagerReaderOnUpdated;
    }

    private void InitReaders()
    {
        _keyboardReader = new SignaledMemoryReader<CChromaKeyboard>(Constants.CChromaKeyboardFileMapping, Constants.CChromaKeyboardEvent);
        _keyboardReader.Updated += OnKeyboardReaderOnUpdated;

        _mouseReader = new SignaledMemoryReader<CChromaMouse>(Constants.CChromaMouseFileMapping, Constants.CChromaMouseEvent);
        _mouseReader.Updated += OnMouseReaderOnUpdated;

        _mousepadReader = new SignaledMemoryReader<CChromaMousepad>(Constants.CChromaMousepadFileMapping, Constants.CChromaMousepadEvent);
        _mousepadReader.Updated += OnMousepadReaderOnUpdated;

        _keypadReader = new SignaledMemoryReader<CChromaKeypad>(Constants.CChromaKeypadFileMapping, Constants.CChromaKeypadEvent);
        _keypadReader.Updated += OnKeypadReaderOnUpdated;

        _headsetReader = new SignaledMemoryReader<CChromaHeadset>(Constants.CChromaHeadsetFileMapping, Constants.CChromaHeadsetEvent);
        _headsetReader.Updated += OnHeadsetReaderOnUpdated;
        
        _chromaLinkReader = new SignaledMemoryReader<CChromaLink>(Constants.CChromaLinkFileMapping, Constants.CChromaLinkEvent);
        _chromaLinkReader.Updated += OnChromaLinkReaderOnUpdated;
    }

    private void DisposeReaders()
    {
        _keyboardReader!.Updated -= OnKeyboardReaderOnUpdated;
        _keyboardReader.Dispose();
        _mouseReader!.Updated -= OnMouseReaderOnUpdated;
        _mouseReader.Dispose();
        _mousepadReader!.Updated -= OnMousepadReaderOnUpdated;
        _mousepadReader.Dispose();
        _keypadReader!.Updated -= OnKeypadReaderOnUpdated;
        _keypadReader.Dispose();
        _headsetReader!.Updated -= OnHeadsetReaderOnUpdated;
        _headsetReader.Dispose();
        _chromaLinkReader!.Updated -= OnChromaLinkReaderOnUpdated;
        _chromaLinkReader.Dispose();
        _appDataReader!.Updated -= OnAppDataReaderOnUpdated;
        _appDataReader.Dispose();
        _appManagerReader!.Updated -= OnAppManagerReaderOnUpdated;
        _appManagerReader.Dispose();
    }

    private void OnKeyboardReaderOnUpdated(object? sender, CChromaKeyboard keyboard)
    {
        KeyboardUpdated?.Invoke(this, keyboard);
    }

    private void OnMouseReaderOnUpdated(object? sender, CChromaMouse mouse)
    {
        MouseUpdated?.Invoke(this, mouse);
    }

    private void OnMousepadReaderOnUpdated(object? sender, CChromaMousepad mousepad)
    {
        MousepadUpdated?.Invoke(this, mousepad);
    }

    private void OnKeypadReaderOnUpdated(object? sender, CChromaKeypad keypad)
    {
        KeypadUpdated?.Invoke(this, keypad);
    }

    private void OnHeadsetReaderOnUpdated(object? sender, CChromaHeadset headset)
    {
        HeadsetUpdated?.Invoke(this, headset);
    }
    
    private void OnChromaLinkReaderOnUpdated(object? sender, CChromaLink chromaLink)
    {
        ChromaLinkUpdated?.Invoke(this, chromaLink);
    }

    private void OnAppDataReaderOnUpdated(object? sender, CAppData appData)
    {
        AppDataUpdated?.Invoke(this, appData);
    }

    private void OnAppManagerReaderOnUpdated(object? sender, CAppManager appManager)
    {
        AppManagerUpdated?.Invoke(this, appManager);
    }

    public void Dispose()
    {
        _disposeEvent?.Set();
        _mutexThread?.Join();
        _disposeEvent?.Dispose();
    }
}