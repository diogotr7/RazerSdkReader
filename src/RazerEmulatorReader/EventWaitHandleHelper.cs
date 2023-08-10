using System.Threading;

namespace RazerEmulatorReader;

public static class EventWaitHandleHelper
{
    public static EventWaitHandle Create(string name) => new(false, EventResetMode.ManualReset, name, out var _);

    public static EventWaitHandle Open(string name) => EventWaitHandle.OpenExisting(name);

    public static void Pulse(string name)
    {
        var e = Open(name);
        e.Pulse();
        e.Close();
    }
}