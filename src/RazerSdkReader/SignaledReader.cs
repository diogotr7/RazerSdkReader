using RazerSdkReader.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RazerSdkReader;

internal sealed class SignaledReader<T> : IDisposable where T : unmanaged
{
    private Action<object?, RazerSdkReaderException>? _exceptionHandler;
    private EventWaitHandle? _eventWaitHandle;
    private MemoryMappedStructReader<T>? _reader;
    private CancellationTokenSource? _cts;
    private Task? _task;

    public event InStructEventHandler<T>? Updated;
    
    public void Start(ReaderDefinition definition, Action<object?, RazerSdkReaderException>? exceptionHandler)
    {
        _exceptionHandler = exceptionHandler;
        _reader = new MemoryMappedStructReader<T>(definition.MemoryMappedFileName);
        _eventWaitHandle = EventWaitHandleHelper.Create(definition.EventWaitHandle);
        _cts = new CancellationTokenSource();
        _task = Task.Run(ReadLoop, _cts.Token);
    }

    private async Task ReadLoop()
    {
        var data = _reader!.Read();
        Updated?.Invoke(this, in data);
        
        _eventWaitHandle!.Reset();

        try
        {
            while (!_cts!.IsCancellationRequested)
            {
                // try to wait synchronously for 5 seconds,
                // if it times out, then wait asynchronously.
                // hopefully this is a good compromise between
                // performance and responsiveness.
                await _eventWaitHandle!.WaitOneAsync(5000, _cts.Token);
                data = _reader!.Read();
                Updated?.Invoke(this, in data);
            }
        }
        catch (TaskCanceledException)
        {
            // ignore
        }
        catch (Exception e)
        {
            _exceptionHandler?.Invoke(this, new RazerSdkReaderException("ReadLoop Error", e));
        }
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _task?.Wait();
        _cts?.Dispose();
        _task?.Dispose();
        _reader?.Dispose();
        _eventWaitHandle?.Dispose();
        Updated = null;
        _exceptionHandler = null;
    }
}