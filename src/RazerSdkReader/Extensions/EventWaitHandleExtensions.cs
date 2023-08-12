using System;
using System.Threading;
using System.Threading.Tasks;

namespace RazerSdkReader.Extensions;

internal static class EventWaitHandleExtensions
{
    public static void Pulse(this EventWaitHandle e)
    {
        e.Set();
        e.Reset();
    }

    public static ValueTask<bool> WaitOneAsync(this EventWaitHandle handle, TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        // Handle synchronous cases.
        var alreadySignalled = handle.WaitOne((int)timeout.TotalMilliseconds);
        if (alreadySignalled)
            return ValueTask.FromResult(true);

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