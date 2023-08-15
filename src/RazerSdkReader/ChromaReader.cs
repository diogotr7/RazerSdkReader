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
public sealed class ChromaReader : IDisposable
{
    private readonly Queue<Mutex> _mutexes = new();
    private TaskCompletionSource? _initCompletionSource;
    private AutoResetEvent? _disposeEvent;
    private Thread? _mutexThread;

    private SignaledReader<ChromaKeyboard>? _keyboardReader;
    private SignaledReader<ChromaMouse>? _mouseReader;
    private SignaledReader<ChromaMousepad>? _mousepadReader;
    private SignaledReader<ChromaKeypad>? _keypadReader;
    private SignaledReader<ChromaHeadset>? _headsetReader;
    private SignaledReader<ChromaLink>? _chromaLinkReader;
    private SignaledReader<ChromaAppData>? _appDataReader;
    private SignaledReader<ChromaAppManager>? _appManagerReader;

    public event EventHandler<ChromaKeyboard>? KeyboardUpdated;
    public event EventHandler<ChromaMouse>? MouseUpdated;
    public event EventHandler<ChromaMousepad>? MousepadUpdated;
    public event EventHandler<ChromaKeypad>? KeypadUpdated;
    public event EventHandler<ChromaHeadset>? HeadsetUpdated;
    public event EventHandler<ChromaLink>? ChromaLinkUpdated;
    public event EventHandler<ChromaAppData>? AppDataUpdated;
    public event EventHandler<ChromaAppManager>? AppManagerUpdated;
    public event EventHandler<RazerSdkReaderException>? Exception;

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
        _mutexThread.Name = "RazerSdkReader Mutex Thread";
        _mutexThread.Start();

        _initCompletionSource.Task.Wait();
    }

    private void Thread()
    {
        try
        {
            InitReaders();

            InitMutexes();
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
        _mutexes.Enqueue(MutexHelper.CreateMutex(Constants.SynapseOnlineMutex));
        EventWaitHandleHelper.Pulse(Constants.SynapseEvent);
        _mutexes.Enqueue(MutexHelper.CreateMutex(Constants.OldSynapseOnlineMutex));
        EventWaitHandleHelper.Pulse(Constants.SynapseEvent);
        _mutexes.Enqueue(MutexHelper.CreateMutex(Constants.OldSynapseVersionMutex));
        EventWaitHandleHelper.Pulse(Constants.SynapseEvent);
        _mutexes.Enqueue(MutexHelper.CreateMutex(Constants.ChromaEmulatorMutex));
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
        _keyboardReader = new SignaledReader<ChromaKeyboard>(Constants.KeyboardFileName, Constants.KeyboardWaitHandle);
        _keyboardReader.Updated += OnKeyboardReaderOnUpdated;
        _keyboardReader.Exception += OnReaderException;
        _keyboardReader.Start();

        _mouseReader = new SignaledReader<ChromaMouse>(Constants.MouseFileName, Constants.MouseWaitHandle);
        _mouseReader.Updated += OnMouseReaderOnUpdated;
        _mouseReader.Exception += OnReaderException;
        _mouseReader.Start();

        _mousepadReader = new SignaledReader<ChromaMousepad>(Constants.MousepadFileName, Constants.MousepadWaitHandle);
        _mousepadReader.Updated += OnMousepadReaderOnUpdated;
        _mousepadReader.Exception += OnReaderException;
        _mousepadReader.Start();

        _keypadReader = new SignaledReader<ChromaKeypad>(Constants.KeypadFileName, Constants.KeypadWaitHandle);
        _keypadReader.Updated += OnKeypadReaderOnUpdated;
        _keypadReader.Exception += OnReaderException;
        _keypadReader.Start();

        _headsetReader = new SignaledReader<ChromaHeadset>(Constants.HeadsetFileName, Constants.HeadsetWaitHandle);
        _headsetReader.Updated += OnHeadsetReaderOnUpdated;
        _headsetReader.Exception += OnReaderException;
        _headsetReader.Start();

        _chromaLinkReader = new SignaledReader<ChromaLink>(Constants.LinkFileName, Constants.LinkWaitHandle);
        _chromaLinkReader.Updated += OnChromaLinkReaderOnUpdated;
        _chromaLinkReader.Exception += OnReaderException;
        _chromaLinkReader.Start();

        _appDataReader = new SignaledReader<ChromaAppData>(Constants.AppDataFileName, Constants.AppDataWaitHandle);
        _appDataReader.Updated += OnAppDataReaderOnUpdated;
        _appDataReader.Exception += OnReaderException;
        _appDataReader.Start();

        _appManagerReader = new SignaledReader<ChromaAppManager>(Constants.AppManagerFileName, Constants.AppManagerWaitHandle);
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

    private void OnReaderException(object? sender, RazerSdkReaderException e)
    {
        Exception?.Invoke(sender, e);
    }

    private void OnKeyboardReaderOnUpdated(object? sender, ChromaKeyboard keyboard)
    {
        KeyboardUpdated?.Invoke(sender, keyboard);
    }

    private void OnMouseReaderOnUpdated(object? sender, ChromaMouse mouse)
    {
        MouseUpdated?.Invoke(sender, mouse);
    }

    private void OnMousepadReaderOnUpdated(object? sender, ChromaMousepad mousepad)
    {
        MousepadUpdated?.Invoke(sender, mousepad);
    }

    private void OnKeypadReaderOnUpdated(object? sender, ChromaKeypad keypad)
    {
        KeypadUpdated?.Invoke(sender, keypad);
    }

    private void OnHeadsetReaderOnUpdated(object? sender, ChromaHeadset headset)
    {
        HeadsetUpdated?.Invoke(sender, headset);
    }

    private void OnChromaLinkReaderOnUpdated(object? sender, ChromaLink chromaLink)
    {
        ChromaLinkUpdated?.Invoke(sender, chromaLink);
    }

    private void OnAppDataReaderOnUpdated(object? sender, ChromaAppData appData)
    {
        AppDataUpdated?.Invoke(sender, appData);
    }

    private void OnAppManagerReaderOnUpdated(object? sender, ChromaAppManager appManager)
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