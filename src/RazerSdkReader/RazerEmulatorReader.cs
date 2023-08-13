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
    public event EventHandler<Exception>? Exception;
    
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
        _keyboardReader.Exception += OnReaderException;
        _keyboardReader.Start();

        _mouseReader = new SignaledReader<CChromaMouse>(Constants.CChromaMouseFileMapping, Constants.CChromaMouseEvent);
        _mouseReader.Updated += OnMouseReaderOnUpdated;
        _mouseReader.Exception += OnReaderException;
        _mouseReader.Start();

        _mousepadReader = new SignaledReader<CChromaMousepad>(Constants.CChromaMousepadFileMapping, Constants.CChromaMousepadEvent);
        _mousepadReader.Updated += OnMousepadReaderOnUpdated;
        _mousepadReader.Exception += OnReaderException;
        _mousepadReader.Start();

        _keypadReader = new SignaledReader<CChromaKeypad>(Constants.CChromaKeypadFileMapping, Constants.CChromaKeypadEvent);
        _keypadReader.Updated += OnKeypadReaderOnUpdated;
        _keypadReader.Exception += OnReaderException;
        _keypadReader.Start();

        _headsetReader = new SignaledReader<CChromaHeadset>(Constants.CChromaHeadsetFileMapping, Constants.CChromaHeadsetEvent);
        _headsetReader.Updated += OnHeadsetReaderOnUpdated;
        _headsetReader.Exception += OnReaderException;
        _headsetReader.Start();
        
        _chromaLinkReader = new SignaledReader<CChromaLink>(Constants.CChromaLinkFileMapping, Constants.CChromaLinkEvent);
        _chromaLinkReader.Updated += OnChromaLinkReaderOnUpdated;
        _chromaLinkReader.Exception += OnReaderException;
        _chromaLinkReader.Start();
        
        _appDataReader = new SignaledReader<CAppData>(Constants.CAppDataFileMapping, Constants.CAppDataEvent);
        _appDataReader.Updated += OnAppDataReaderOnUpdated;
        _appDataReader.Exception += OnReaderException;
        _appDataReader.Start();
        
        _appManagerReader = new SignaledReader<CAppManager>(Constants.CAppManagerFileMapping, Constants.CAppManagerEvent);
        _appManagerReader.Updated += OnAppManagerReaderOnUpdated;
        _appManagerReader.Exception += OnReaderException;
        _appManagerReader.Start();
    }

    private void DisposeReaders()
    {
        _keyboardReader!.Updated -= OnKeyboardReaderOnUpdated;
        _keyboardReader!.Exception -= OnReaderException;
        _keyboardReader.Dispose();
        _mouseReader!.Updated -= OnMouseReaderOnUpdated;
        _mouseReader!.Exception -= OnReaderException;
        _mouseReader.Dispose();
        _mousepadReader!.Updated -= OnMousepadReaderOnUpdated;
        _mousepadReader!.Exception -= OnReaderException;
        _mousepadReader.Dispose();
        _keypadReader!.Updated -= OnKeypadReaderOnUpdated;
        _keypadReader!.Exception -= OnReaderException;
        _keypadReader.Dispose();
        _headsetReader!.Updated -= OnHeadsetReaderOnUpdated;
        _headsetReader!.Exception -= OnReaderException;
        _headsetReader.Dispose();
        _chromaLinkReader!.Updated -= OnChromaLinkReaderOnUpdated;
        _chromaLinkReader!.Exception -= OnReaderException;
        _chromaLinkReader.Dispose();
        _appDataReader!.Updated -= OnAppDataReaderOnUpdated;
        _appDataReader!.Exception -= OnReaderException;
        _appDataReader.Dispose();
        _appManagerReader!.Updated -= OnAppManagerReaderOnUpdated;
        _appManagerReader!.Exception -= OnReaderException;
        _appManagerReader.Dispose();
    }
    
    private void OnReaderException(object? sender, Exception e)
    {
        Exception?.Invoke(sender, e);
    }

    private void OnKeyboardReaderOnUpdated(object? sender, CChromaKeyboard keyboard)
    {
        KeyboardUpdated?.Invoke(sender, keyboard);
    }

    private void OnMouseReaderOnUpdated(object? sender, CChromaMouse mouse)
    {
        MouseUpdated?.Invoke(sender, mouse);
    }

    private void OnMousepadReaderOnUpdated(object? sender, CChromaMousepad mousepad)
    {
        MousepadUpdated?.Invoke(sender, mousepad);
    }

    private void OnKeypadReaderOnUpdated(object? sender, CChromaKeypad keypad)
    {
        KeypadUpdated?.Invoke(sender, keypad);
    }

    private void OnHeadsetReaderOnUpdated(object? sender, CChromaHeadset headset)
    {
        HeadsetUpdated?.Invoke(sender, headset);
    }
    
    private void OnChromaLinkReaderOnUpdated(object? sender, CChromaLink chromaLink)
    {
        ChromaLinkUpdated?.Invoke(sender, chromaLink);
    }

    private void OnAppDataReaderOnUpdated(object? sender, CAppData appData)
    {
        AppDataUpdated?.Invoke(sender, appData);
    }

    private void OnAppManagerReaderOnUpdated(object? sender, CAppManager appManager)
    {
        AppManagerUpdated?.Invoke(sender, appManager);
    }

    public void Dispose()
    {
        _disposeEvent?.Set();
        _mutexThread?.Join();
        _disposeEvent?.Dispose();
    }
}