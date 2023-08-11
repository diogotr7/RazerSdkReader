using System;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using RazerEmulatorReader.Extensions;

namespace RazerEmulatorReader;

[SupportedOSPlatform("windows")]
public sealed class SignaledReader<T> : IDisposable where T : unmanaged
{
    private readonly EventWaitHandle _eventWaitHandle;
    private readonly MemoryMappedStructReader<T> _reader;
    private CancellationTokenSource? _cts;
    private Task? _task;

    public SignaledReader(string mmf, string eventWaitHandle)
    {
        _reader = new MemoryMappedStructReader<T>(mmf);
        _eventWaitHandle = EventWaitHandleHelper.Create(eventWaitHandle);
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

    public event EventHandler<T>? OnRead;

    public void Start()
    {
        _cts = new CancellationTokenSource();
        _task = Task.Run(ReadLoop, _cts.Token);
    }

    private async Task ReadLoop()
    {
        _eventWaitHandle.Reset();

        while (!_cts!.IsCancellationRequested)
        {
            // sdk runs at 30fps, let's wait for 40ms per frame.
            // If it takes longer than that, we will asynchrounously wait for the next frame.
            await _eventWaitHandle.WaitOneAsync(40);
            OnRead?.Invoke(this, _reader.Read());
        }
    }
}