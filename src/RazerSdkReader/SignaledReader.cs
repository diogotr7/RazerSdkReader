using System;
using System.Threading;
using System.Threading.Tasks;
using RazerSdkReader.Extensions;

namespace RazerSdkReader;

internal sealed class SignaledReader<T> : IDisposable where T : unmanaged
{
    private readonly EventWaitHandle _eventWaitHandle;
    private readonly MemoryMappedStructReader<T> _reader;
    private CancellationTokenSource? _cts;
    private Task? _task;

    public event InStructEventHandler<T>? Updated;
    public event EventHandler<RazerSdkReaderException>? Exception;

    public SignaledReader(string memoryMappedFileName, string eventWaitHandle)
    {
        _reader = new MemoryMappedStructReader<T>(memoryMappedFileName);
        _eventWaitHandle = EventWaitHandleHelper.Create(eventWaitHandle);
    }

    public void Start()
    {
        _cts = new CancellationTokenSource();
        _task = Task.Run(ReadLoop, _cts.Token);
    }

    private async Task ReadLoop()
    {
        _eventWaitHandle.Reset();

        try
        {
            while (!_cts!.IsCancellationRequested)
            {
                // try to wait synchronously for 5 seconds,
                // if it times out, then wait asynchronously.
                // hopefully this is a good compromise between
                // performance and responsiveness.
                await _eventWaitHandle.WaitOneAsync(TimeSpan.FromSeconds(5), _cts.Token);
                var data = _reader.Read();
                Updated?.Invoke(this, in data);
            }
        }
        catch (TaskCanceledException)
        {
            // ignore
        }
        catch (Exception e)
        {
            Exception?.Invoke(this, new RazerSdkReaderException("ReadLoop Error", e));
        }
    }
    
    internal T Read()
    {
        return _reader.Read();
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _task?.Wait();
        _cts?.Dispose();
        _task?.Dispose();
        _reader.Dispose();
        _eventWaitHandle.Dispose();
    }
}