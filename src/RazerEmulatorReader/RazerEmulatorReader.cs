using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using RazerEmulatorReader.Extensions;
using RazerEmulatorReader.Structures;

namespace RazerEmulatorReader;

[SupportedOSPlatform("windows")]
public sealed class RazerEmulatorReader : IDisposable
{
    private readonly Queue<Mutex> _mutexes = new();
    private ManualResetEventSlim? _initializationEvent;
    private Thread? _mutexThread;
    
    private SignaledReader<CChromaKeyboard>? _keyboardReader;
    private SignaledReader<CChromaMouse>? _mouseReader;
    private SignaledReader<CChromaMousepad>? _mousepadReader;
    private SignaledReader<CChromaKeypad>? _keypadReader;
    private SignaledReader<CChromaHeadset>? _headsetReader;
    
    public event EventHandler<CChromaKeyboard> KeyboardUpdated;
    public event EventHandler<CChromaMouse> MouseUpdated;
    public event EventHandler<CChromaMousepad> MousepadUpdated;
    public event EventHandler<CChromaKeypad> KeypadUpdated;
    public event EventHandler<CChromaHeadset> HeadsetUpdated;
    
    public void Start()
    {
        var isServiceRunning = Process.GetProcessesByName("RzSDKService").Any();
        
        if (!isServiceRunning)
            throw new InvalidOperationException("RzSdkService is not running.");
        
        _initializationEvent = new ManualResetEventSlim();
        _mutexThread = new Thread(Thread);
        _mutexThread.Start();
        SpinWait.SpinUntil(() => _mutexThread.IsAlive);
    }

    private void Thread()
    {
        InitMutexes();

        InitReaders();

        _initializationEvent?.Wait();
        
        DisposeReaders();

        DisposeMutexes();
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

    public void Dispose()
    {
        _initializationEvent?.Set();
        _mutexThread?.Join();
        _initializationEvent?.Dispose();
    }
}