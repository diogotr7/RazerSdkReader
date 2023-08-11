using System.Runtime.Versioning;
using System.Threading;

namespace RazerEmulatorReader.Extensions;

public static class EventWaitHandleHelper
{
    public static EventWaitHandle Create(string name)
    {
        return new EventWaitHandle(false, EventResetMode.ManualReset, name, out var _);
    }

    [SupportedOSPlatform("windows")]
    public static EventWaitHandle Open(string name)
    {
        return EventWaitHandle.OpenExisting(name);
    }

    [SupportedOSPlatform("windows")]
    public static void Pulse(string name)
    {
        var e = Open(name);
        e.Pulse();
        e.Close();
    }
}