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

    public static ValueTask<bool> WaitOneAsync(this EventWaitHandle handle, TimeSpan syncTimeout, CancellationToken cancellationToken = default)
    {
        // Handle synchronous cases.
        var alreadySignalled = handle.WaitOne((int)syncTimeout.TotalMilliseconds);
        if (alreadySignalled)
            return ValueTask.FromResult(true);

        return handle.WaitOneAsync(cancellationToken);
    }
    
    //separate async case to avoid closure allocation in the common case
    private static ValueTask<bool> WaitOneAsync(this EventWaitHandle handle, CancellationToken cancellationToken = default)
    {
        // Register all asynchronous cases.
        var tcs = new TaskCompletionSource<bool>();
        var threadPoolRegistration = ThreadPool.RegisterWaitForSingleObject(
            handle,
            static (state, timedOut) => ((TaskCompletionSource<bool>)state!).TrySetResult(!timedOut),
            tcs,
            -1,
            true);
        cancellationToken.Register(() => tcs.TrySetCanceled(), useSynchronizationContext: false);
        tcs.Task.ContinueWith(_ => threadPoolRegistration.Unregister(null), TaskScheduler.Default);
        return new ValueTask<bool>(tcs.Task);
    }
}