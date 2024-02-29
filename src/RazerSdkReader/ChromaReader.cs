using RazerSdkReader.Structures;
using System;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace RazerSdkReader;

[SupportedOSPlatform("windows")]
public sealed class ChromaReader : IDisposable
{
    private TaskCompletionSource? _initCompletionSource;
    private AutoResetEvent? _disposeEvent;
    private ChromaMutex? _chromaMutex;
    private Thread? _mutexThread;

    private SignaledReader<ChromaAppData>? _appDataReader;
    private SignaledReader<ChromaKeyboard>? _keyboardReader;
    private SignaledReader<ChromaMouse>? _mouseReader;
    private SignaledReader<ChromaMousepad>? _mousepadReader;
    private SignaledReader<ChromaKeypad>? _keypadReader;
    private SignaledReader<ChromaHeadset>? _headsetReader;
    private SignaledReader<ChromaLink>? _chromaLinkReader;

    private InStructEventHandler<ChromaAppData>? _appDataUpdated;
    private InStructEventHandler<ChromaKeyboard>? _keyboardUpdated;
    private InStructEventHandler<ChromaMouse>? _mouseUpdated;
    private InStructEventHandler<ChromaMousepad>? _mousepadUpdated;
    private InStructEventHandler<ChromaKeypad>? _keypadUpdated;
    private InStructEventHandler<ChromaHeadset>? _headsetUpdated;
    private InStructEventHandler<ChromaLink>? _chromaLinkUpdated;

    public event InStructEventHandler<ChromaAppData>? AppDataUpdated
    {
        add
        {
            if (!EnableAppData)
                throw new InvalidOperationException("AppData is not enabled, this event will never fire.");

            _appDataUpdated += value;
        }

        remove => _appDataUpdated -= value;
    }

    public event InStructEventHandler<ChromaKeyboard>? KeyboardUpdated
    {
        add
        {
            if (!EnableKeyboard)
                throw new InvalidOperationException("Keyboard is not enabled, this event will never fire.");

            _keyboardUpdated += value;
        }

        remove => _keyboardUpdated -= value;
    }

    public event InStructEventHandler<ChromaMouse>? MouseUpdated
    {
        add
        {
            if (!EnableMouse)
                throw new InvalidOperationException("Mouse is not enabled, this event will never fire.");

            _mouseUpdated += value;
        }

        remove => _mouseUpdated -= value;
    }

    public event InStructEventHandler<ChromaMousepad>? MousepadUpdated
    {
        add
        {
            if (!EnableMousepad)
                throw new InvalidOperationException("Mousepad is not enabled, this event will never fire.");

            _mousepadUpdated += value;
        }

        remove => _mousepadUpdated -= value;
    }

    public event InStructEventHandler<ChromaKeypad>? KeypadUpdated
    {
        add
        {
            if (!EnableKeypad)
                throw new InvalidOperationException("Keypad is not enabled, this event will never fire.");

            _keypadUpdated += value;
        }

        remove => _keypadUpdated -= value;
    }

    public event InStructEventHandler<ChromaHeadset>? HeadsetUpdated
    {
        add
        {
            if (!EnableHeadset)
                throw new InvalidOperationException("Headset is not enabled, this event will never fire.");

            _headsetUpdated += value;
        }

        remove => _headsetUpdated -= value;
    }

    public event InStructEventHandler<ChromaLink>? ChromaLinkUpdated
    {
        add
        {
            if (!EnableChromaLink)
                throw new InvalidOperationException("ChromaLink is not enabled, this event will never fire.");

            _chromaLinkUpdated += value;
        }

        remove => _chromaLinkUpdated -= value;
    }

    public event EventHandler<RazerSdkReaderException>? Exception;

    public bool EnableAppData { get; init; } = true;
    public bool EnableKeyboard { get; init; } = true;
    public bool EnableMouse { get; init; } = true;
    public bool EnableMousepad { get; init; } = true;
    public bool EnableKeypad { get; init; } = true;
    public bool EnableHeadset { get; init; } = true;
    public bool EnableChromaLink { get; init; } = true;

    public void Start()
    {
        if (!EnableAppData && !EnableKeyboard && !EnableMouse && !EnableMousepad && !EnableKeypad && !EnableHeadset && !EnableChromaLink)
            throw new InvalidOperationException("No devices are enabled.");

        if (_mutexThread is { IsAlive: true })
            return;

        var isServiceRunning = Process.GetProcessesByName("RzSDKService").Length != 0;

        if (!isServiceRunning)
            throw new InvalidOperationException("RzSdkService is not running.");

        _disposeEvent = new(false);
        _initCompletionSource = new();
        _mutexThread = new Thread(Thread)
        {
            Name = "RazerSdkReader Mutex Thread"
        };
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

    private void InitReaders()
    {
        if (EnableAppData)
        {
            _appDataReader = new SignaledReader<ChromaAppData>(Constants.AppDataFileName, Constants.AppDataWaitHandle);
            OnAppDataReaderOnUpdated(this, _appDataReader.Read());
            _appDataReader.Updated += OnAppDataReaderOnUpdated;
            _appDataReader.Exception += OnReaderException;
            _appDataReader.Start();
        }

        if (EnableKeyboard)
        {
            _keyboardReader = new SignaledReader<ChromaKeyboard>(Constants.KeyboardFileName, Constants.KeyboardWaitHandle);
            OnKeyboardReaderOnUpdated(this, _keyboardReader.Read());
            _keyboardReader.Updated += OnKeyboardReaderOnUpdated;
            _keyboardReader.Exception += OnReaderException;
            _keyboardReader.Start();
        }

        if (EnableMouse)
        {
            _mouseReader = new SignaledReader<ChromaMouse>(Constants.MouseFileName, Constants.MouseWaitHandle);
            OnMouseReaderOnUpdated(this, _mouseReader.Read());
            _mouseReader.Updated += OnMouseReaderOnUpdated;
            _mouseReader.Exception += OnReaderException;
            _mouseReader.Start();
        }

        if (EnableMousepad)
        {
            _mousepadReader = new SignaledReader<ChromaMousepad>(Constants.MousepadFileName, Constants.MousepadWaitHandle);
            OnMousepadReaderOnUpdated(this, _mousepadReader.Read());
            _mousepadReader.Updated += OnMousepadReaderOnUpdated;
            _mousepadReader.Exception += OnReaderException;
            _mousepadReader.Start();
        }

        if (EnableKeypad)
        {
            _keypadReader = new SignaledReader<ChromaKeypad>(Constants.KeypadFileName, Constants.KeypadWaitHandle);
            OnKeypadReaderOnUpdated(this, _keypadReader.Read());
            _keypadReader.Updated += OnKeypadReaderOnUpdated;
            _keypadReader.Exception += OnReaderException;
            _keypadReader.Start();
        }

        if (EnableHeadset)
        {
            _headsetReader = new SignaledReader<ChromaHeadset>(Constants.HeadsetFileName, Constants.HeadsetWaitHandle);
            OnHeadsetReaderOnUpdated(this, _headsetReader.Read());
            _headsetReader.Updated += OnHeadsetReaderOnUpdated;
            _headsetReader.Exception += OnReaderException;
            _headsetReader.Start();
        }

        if (EnableChromaLink)
        {
            _chromaLinkReader = new SignaledReader<ChromaLink>(Constants.LinkFileName, Constants.LinkWaitHandle);
            OnChromaLinkReaderOnUpdated(this, _chromaLinkReader.Read());
            _chromaLinkReader.Updated += OnChromaLinkReaderOnUpdated;
            _chromaLinkReader.Exception += OnReaderException;
            _chromaLinkReader.Start();
        }
    }

    private void DisposeReaders()
    {
        if (_appDataReader is not null)
        {
            _appDataReader.Updated -= OnAppDataReaderOnUpdated;
            _appDataReader.Exception -= OnReaderException;
            _appDataReader.Dispose();
        }
        if (_keyboardReader != null)
        {
            _keyboardReader!.Updated -= OnKeyboardReaderOnUpdated;
            _keyboardReader!.Exception -= OnReaderException;
            _keyboardReader.Dispose();
        }
        if (_mouseReader is not null)
        {
            _mouseReader!.Updated -= OnMouseReaderOnUpdated;
            _mouseReader!.Exception -= OnReaderException;
            _mouseReader.Dispose();
        }
        if (_mousepadReader is not null)
        {
            _mousepadReader!.Updated -= OnMousepadReaderOnUpdated;
            _mousepadReader!.Exception -= OnReaderException;
            _mousepadReader.Dispose();
        }
        if (_keypadReader is not null)
        {
            _keypadReader!.Updated -= OnKeypadReaderOnUpdated;
            _keypadReader!.Exception -= OnReaderException;
            _keypadReader.Dispose();
        }
        if (_headsetReader is not null)
        {
            _headsetReader!.Updated -= OnHeadsetReaderOnUpdated;
            _headsetReader!.Exception -= OnReaderException;
            _headsetReader.Dispose();
        }
        if (_chromaLinkReader is not null)
        {
            _chromaLinkReader!.Updated -= OnChromaLinkReaderOnUpdated;
            _chromaLinkReader!.Exception -= OnReaderException;
            _chromaLinkReader.Dispose();
        }
    }

    private void OnReaderException(object? sender, RazerSdkReaderException e)
    {
        Exception?.Invoke(sender, e);
    }

    private void OnAppDataReaderOnUpdated(object? sender, in ChromaAppData appData)
    {
        _appDataUpdated?.Invoke(sender, in appData);
    }

    private void OnKeyboardReaderOnUpdated(object? sender, in ChromaKeyboard keyboard)
    {
        _keyboardUpdated?.Invoke(sender, in keyboard);
    }

    private void OnMouseReaderOnUpdated(object? sender, in ChromaMouse mouse)
    {
        _mouseUpdated?.Invoke(sender, in mouse);
    }

    private void OnMousepadReaderOnUpdated(object? sender, in ChromaMousepad mousepad)
    {
        _mousepadUpdated?.Invoke(sender, in mousepad);
    }

    private void OnKeypadReaderOnUpdated(object? sender, in ChromaKeypad keypad)
    {
        _keypadUpdated?.Invoke(sender, in keypad);
    }

    private void OnHeadsetReaderOnUpdated(object? sender, in ChromaHeadset headset)
    {
        _headsetUpdated?.Invoke(sender, in headset);
    }

    private void OnChromaLinkReaderOnUpdated(object? sender, in ChromaLink chromaLink)
    {
        _chromaLinkUpdated?.Invoke(sender, in chromaLink);
    }

    public void Dispose()
    {
        _disposeEvent?.Set();
        _mutexThread?.Join();
        _disposeEvent?.Dispose();
    }
}