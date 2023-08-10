using System.Threading;

namespace RazerEmulatorReader;

public static class EventWaitHandleExtensions
{
    public static void Pulse(this EventWaitHandle e)
    {
        e.Set();
        e.Reset();
    }
}