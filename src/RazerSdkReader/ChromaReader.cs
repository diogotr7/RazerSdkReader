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
    private ManualResetEvent? _disposeEvent;
    private ChromaMutex? _chromaMutex;
    private Thread? _mutexThread;

    private readonly SignaledReader<ChromaAppData> _appDataReader = new();
    private readonly SignaledReader<ChromaKeyboard> _keyboardReader = new();
    private readonly SignaledReader<ChromaMouse> _mouseReader = new();
    private readonly SignaledReader<ChromaMousepad> _mousepadReader = new();
    private readonly SignaledReader<ChromaKeypad> _keypadReader = new();
    private readonly SignaledReader<ChromaHeadset> _headsetReader = new();
    private readonly SignaledReader<ChromaLink> _chromaLinkReader = new();

    private static ChromaReader? _instance;
    public static ChromaReader Instance => _instance ??= new ChromaReader();

    public event EventHandler<RazerSdkReaderException>? Exception;
    public event InStructEventHandler<ChromaAppData>? AppDataUpdated
    {
        add => Subscribe(EnableAppData, _appDataReader, value);
        remove => _appDataReader.Updated -= value;
    }
    public event InStructEventHandler<ChromaKeyboard>? KeyboardUpdated
    {
        add => Subscribe(EnableKeyboard, _keyboardReader, value);
        remove => _keyboardReader.Updated -= value;
    }
    public event InStructEventHandler<ChromaMouse>? MouseUpdated
    {
        add => Subscribe(EnableMouse, _mouseReader, value);
        remove => _mouseReader.Updated -= value;
    }
    public event InStructEventHandler<ChromaMousepad>? MousepadUpdated
    {
        add => Subscribe(EnableMousepad, _mousepadReader, value);
        remove => _mousepadReader.Updated -= value;
    }
    public event InStructEventHandler<ChromaKeypad>? KeypadUpdated
    {
        add => Subscribe(EnableKeypad, _keypadReader, value);
        remove => _keypadReader.Updated -= value;
    }
    public event InStructEventHandler<ChromaHeadset>? HeadsetUpdated
    {
        add => Subscribe(EnableHeadset, _headsetReader, value);
        remove => _headsetReader.Updated -= value;
    }
    public event InStructEventHandler<ChromaLink>? ChromaLinkUpdated
    {
        add => Subscribe(EnableChromaLink, _chromaLinkReader, value);
        remove => _chromaLinkReader.Updated -= value;
    }

    public bool EnableAppData { get; init; } = true;
    public bool EnableKeyboard { get; init; } = true;
    public bool EnableMouse { get; init; } = true;
    public bool EnableMousepad { get; init; } = true;
    public bool EnableKeypad { get; init; } = true;
    public bool EnableHeadset { get; init; } = true;
    public bool EnableChromaLink { get; init; } = true;

    public ChromaReader()
    {
        if (_instance is not null)
            throw new InvalidOperationException(
                "Only one instance of ChromaReader can be created. Dispose the current instance before creating a new one.");

        _instance = this;
    }

    public void Start()
    {
        if (!EnableAppData && !EnableKeyboard && !EnableMouse && !EnableMousepad && !EnableKeypad && !EnableHeadset && !EnableChromaLink)
            throw new InvalidOperationException("No devices are enabled.");

        if (_mutexThread is { IsAlive: true })
            return;

        if (Process.GetProcessesByName("RzSDKService").Length == 0)
            throw new InvalidOperationException("RzSdkService is not running.");

        _disposeEvent = new ManualResetEvent(false);
        _initCompletionSource = new TaskCompletionSource();
        _mutexThread = new Thread(MutexThread)
        {
            Name = "RazerSdkReader Mutex Thread"
        };
        _mutexThread.Start();

        _initCompletionSource.Task.Wait();
    }

    private void MutexThread()
    {
        Action<object?, RazerSdkReaderException> onReaderException = (sender, e) => Exception?.Invoke(sender, e);
        try
        {
            _chromaMutex = new ChromaMutex();
            if (EnableAppData) _appDataReader.Start(ReaderDefinitions.AppData, onReaderException);
            if (EnableKeyboard) _keyboardReader.Start(ReaderDefinitions.Keyboard, onReaderException);
            if (EnableMouse) _mouseReader.Start(ReaderDefinitions.Mouse, onReaderException);
            if (EnableMousepad) _mousepadReader.Start(ReaderDefinitions.Mousepad, onReaderException);
            if (EnableKeypad) _keypadReader.Start(ReaderDefinitions.Keypad, onReaderException);
            if (EnableHeadset) _headsetReader.Start(ReaderDefinitions.Headset, onReaderException);
            if (EnableChromaLink) _chromaLinkReader.Start(ReaderDefinitions.Link, onReaderException);
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
            _appDataReader.Dispose();
            _keyboardReader.Dispose();
            _mouseReader.Dispose();
            _mousepadReader.Dispose();
            _keypadReader.Dispose();
            _headsetReader.Dispose();
            _chromaLinkReader.Dispose();
            _chromaMutex?.Dispose();
        }
        catch
        {
            //dispose errors are not important
        }
    }

    private static void Subscribe<T>(bool enable, SignaledReader<T> store, InStructEventHandler<T>? value) where T : unmanaged
    {
        if (!enable) throw new InvalidOperationException($"{typeof(T).Name} is not enabled, this event will never fire.");
        store.Updated += value;
    }

    public void Dispose()
    {
        _disposeEvent?.Set();
        _mutexThread?.Join();
        _disposeEvent?.Dispose();
        _initCompletionSource = null;
        _instance = null;
    }
}