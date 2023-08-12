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
    
    private SignaledReader<CChromaKeyboard>? _keyboardReader;
    private SignaledReader<CChromaMouse>? _mouseReader;
    private SignaledReader<CChromaMousepad>? _mousepadReader;
    private SignaledReader<CChromaKeypad>? _keypadReader;
    private SignaledReader<CChromaHeadset>? _headsetReader;
    private SignaledReader<CChromaLink>? _chromaLinkReader;
    private SignaledReader<CAppData>? _appDataReader;
    private SignaledReader<CAppManager>? _appManagerReader;
    
    public event EventHandler<CChromaKeyboard>? KeyboardUpdated;
    public event EventHandler<CChromaMouse>? MouseUpdated;
    public event EventHandler<CChromaMousepad>? MousepadUpdated;
    public event EventHandler<CChromaKeypad>? KeypadUpdated;
    public event EventHandler<CChromaHeadset>? HeadsetUpdated;
    public event EventHandler<CChromaLink>? ChromaLinkUpdated;
    public event EventHandler<CAppData>? AppDataUpdated;
    public event EventHandler<CAppManager>? AppManagerUpdated;
    
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
        try
        {
            InitMutexes();
            InitReaders();
        }
        catch (Exception ex)
        {
            _initCompletionSource?.TrySetException(new InvalidOperationException("Failed to initialize RazerSdkReader.", ex));
            return;
        }
        
        _initCompletionSource?.TrySetResult();

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

    private void InitReaders()
    {
        _keyboardReader = new SignaledReader<CChromaKeyboard>(Constants.CChromaKeyboardFileMapping, Constants.CChromaKeyboardEvent);
        _keyboardReader.Updated += OnKeyboardReaderOnUpdated;
        _keyboardReader.Start();

        _mouseReader = new SignaledReader<CChromaMouse>(Constants.CChromaMouseFileMapping, Constants.CChromaMouseEvent);
        _mouseReader.Updated += OnMouseReaderOnUpdated;
        _mouseReader.Start();

        _mousepadReader = new SignaledReader<CChromaMousepad>(Constants.CChromaMousepadFileMapping, Constants.CChromaMousepadEvent);
        _mousepadReader.Updated += OnMousepadReaderOnUpdated;
        _mousepadReader.Start();

        _keypadReader = new SignaledReader<CChromaKeypad>(Constants.CChromaKeypadFileMapping, Constants.CChromaKeypadEvent);
        _keypadReader.Updated += OnKeypadReaderOnUpdated;
        _keypadReader.Start();

        _headsetReader = new SignaledReader<CChromaHeadset>(Constants.CChromaHeadsetFileMapping, Constants.CChromaHeadsetEvent);
        _headsetReader.Updated += OnHeadsetReaderOnUpdated;
        _headsetReader.Start();
        
        _chromaLinkReader = new SignaledReader<CChromaLink>(Constants.CChromaLinkFileMapping, Constants.CChromaLinkEvent);
        _chromaLinkReader.Updated += OnChromaLinkReaderOnUpdated;
        _chromaLinkReader.Start();
        
        _appDataReader = new SignaledReader<CAppData>(Constants.CAppDataFileMapping, Constants.CAppDataEvent);
        _appDataReader.Updated += OnAppDataReaderOnUpdated;
        _appDataReader.Start();
        
        _appManagerReader = new SignaledReader<CAppManager>(Constants.CAppManagerFileMapping, Constants.CAppManagerEvent);
        _appManagerReader.Updated += OnAppManagerReaderOnUpdated;
        _appManagerReader.Start();
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