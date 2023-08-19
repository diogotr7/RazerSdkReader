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
    private TaskCompletionSource? _initCompletionSource;
    private AutoResetEvent? _disposeEvent;
    private ChromaMutex? _chromaMutex;
    private Thread? _mutexThread;

    private SignaledReader<ChromaKeyboard>? _keyboardReader;
    private SignaledReader<ChromaMouse>? _mouseReader;
    private SignaledReader<ChromaMousepad>? _mousepadReader;
    private SignaledReader<ChromaKeypad>? _keypadReader;
    private SignaledReader<ChromaHeadset>? _headsetReader;
    private SignaledReader<ChromaLink>? _chromaLinkReader;
    private SignaledReader<ChromaAppData>? _appDataReader;
    private SignaledReader<ChromaAppManager>? _appManagerReader;

    public event InStructEventHandler<ChromaKeyboard>? KeyboardUpdated;
    public event InStructEventHandler<ChromaMouse>? MouseUpdated;
    public event InStructEventHandler<ChromaMousepad>? MousepadUpdated;
    public event InStructEventHandler<ChromaKeypad>? KeypadUpdated;
    public event InStructEventHandler<ChromaHeadset>? HeadsetUpdated;
    public event InStructEventHandler<ChromaLink>? ChromaLinkUpdated;
    public event InStructEventHandler<ChromaAppData>? AppDataUpdated;
    public event InStructEventHandler<ChromaAppManager>? AppManagerUpdated;
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
            _chromaMutex = new ChromaMutex();
            InitReaders();
        }
        catch (Exception ex)
        {
            _initCompletionSource?.SetException(new InvalidOperationException("Failed to initialize RazerSdkReader.", ex));
            return;
        }

        _initCompletionSource?.SetResult();

        _disposeEvent?.WaitOne();

        try
        {
            DisposeReaders();
            _chromaMutex.Dispose();
        }
        catch
        {
            //dispose errors are not important
        }
    }

    #region Init readers

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

    #endregion

    #region Stop readers

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

    #endregion

    #region Event Handlers

    private void OnReaderException(object? sender, RazerSdkReaderException e)
    {
        Exception?.Invoke(sender, e);
    }

    private void OnKeyboardReaderOnUpdated(object? sender, in ChromaKeyboard keyboard)
    {
        KeyboardUpdated?.Invoke(sender, in keyboard);
    }

    private void OnMouseReaderOnUpdated(object? sender, in ChromaMouse mouse)
    {
        MouseUpdated?.Invoke(sender, in mouse);
    }

    private void OnMousepadReaderOnUpdated(object? sender, in ChromaMousepad mousepad)
    {
        MousepadUpdated?.Invoke(sender, in mousepad);
    }

    private void OnKeypadReaderOnUpdated(object? sender, in ChromaKeypad keypad)
    {
        KeypadUpdated?.Invoke(sender, in keypad);
    }

    private void OnHeadsetReaderOnUpdated(object? sender, in ChromaHeadset headset)
    {
        HeadsetUpdated?.Invoke(sender, in headset);
    }

    private void OnChromaLinkReaderOnUpdated(object? sender, in ChromaLink chromaLink)
    {
        ChromaLinkUpdated?.Invoke(sender, in chromaLink);
    }

    private void OnAppDataReaderOnUpdated(object? sender, in ChromaAppData appData)
    {
        AppDataUpdated?.Invoke(sender, in appData);
    }

    private void OnAppManagerReaderOnUpdated(object? sender, in ChromaAppManager appManager)
    {
        AppManagerUpdated?.Invoke(sender, in appManager);
    }

    #endregion

    public void Dispose()
    {
        _disposeEvent?.Set();
        _mutexThread?.Join();
        _disposeEvent?.Dispose();
    }
}