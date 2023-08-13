using System;
using System.Runtime.Versioning;
using System.Threading;
using RazerSdkReader.Extensions;

namespace RazerSdkReader;

[SupportedOSPlatform("windows")]
internal abstract class SignaledReader : IDisposable
{
    public readonly EventWaitHandle EventWaitHandle;

    protected SignaledReader(string eventWaitHandle)
    {
        EventWaitHandle = EventWaitHandleHelper.Create(eventWaitHandle);
    }

    internal abstract void Update();

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            EventWaitHandle.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}