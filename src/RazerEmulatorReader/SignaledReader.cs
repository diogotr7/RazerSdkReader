using System;
using System.Threading;
using System.Threading.Tasks;

namespace RazerEmulatorReader;

public sealed class SignaledReader<T> : IDisposable where T : unmanaged
{
    private readonly MemoryMappedStructReader<T> _mmf;
    private readonly EventWaitHandle _eventWaitHandle;
    private CancellationTokenSource? _cts;
    private Task? _task;

    public event EventHandler<T>? OnRead;

    public SignaledReader(string mmf, string eventWaitHandle)
    {
        _mmf = new MemoryMappedStructReader<T>(mmf);
        _eventWaitHandle = EventWaitHandleHelper.Create(eventWaitHandle);
        _eventWaitHandle.Reset();
    }

    public void Start()
    {
        _cts = new CancellationTokenSource();
        _task = Task.Run(ReadLoop, _cts.Token);
    }

    private void ReadLoop()
    {
        while (!_cts.IsCancellationRequested)
        {
            _eventWaitHandle.WaitOne();
            var t = _mmf.Read();
            OnRead?.Invoke(this, t);
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _task.Wait();
        _cts.Dispose();
        _task.Dispose();
        _mmf.Dispose();
        _eventWaitHandle.Dispose();
    }
}