using System.Threading;
using System.Threading.Tasks;

namespace RazerEmulatorReader.Extensions;

public static class EventWaitHandleExtensions
{
    public static void Pulse(this EventWaitHandle e)
    {
        e.Set();
        e.Reset();
    }

    public static ValueTask<bool> WaitOneAsync(this EventWaitHandle handle, int waitMs, long timeout = -1, CancellationToken cancellationToken = default)
    {
        // Handle synchronous cases.
        var alreadySignalled = handle.WaitOne(waitMs);
        if (alreadySignalled)
            return ValueTask.FromResult(true);
        if (timeout == 0)
            return ValueTask.FromResult(false);

        // Register all asynchronous cases.
        var tcs = new TaskCompletionSource<bool>();
        var threadPoolRegistration = ThreadPool.RegisterWaitForSingleObject(
            handle,
            (state, timedOut) => ((TaskCompletionSource<bool>)state!).TrySetResult(!timedOut),
            tcs,
            timeout,
            true);
        cancellationToken.Register(() => tcs.TrySetCanceled(), useSynchronizationContext: false);
        tcs.Task.ContinueWith(_ => threadPoolRegistration.Unregister(null), TaskScheduler.Default);
        return new ValueTask<bool>(tcs.Task);
    }
}