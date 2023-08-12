using System;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using RazerSdkReader.Extensions;

namespace RazerSdkReader;

[SupportedOSPlatform("windows")]
internal sealed class SignaledReader<T> : IDisposable where T : unmanaged
{
    private readonly EventWaitHandle _eventWaitHandle;
    private readonly MemoryMappedStructReader<T> _reader;
    private CancellationTokenSource? _cts;
    private Task? _task;

    public event EventHandler<T>? Updated;

    public SignaledReader(string mmf, string eventWaitHandle)
    {
        _reader = new MemoryMappedStructReader<T>(mmf);
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
                Updated?.Invoke(this, _reader.Read());
            }
        }
        catch
        {

        }
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