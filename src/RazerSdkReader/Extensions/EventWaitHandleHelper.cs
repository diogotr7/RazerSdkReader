using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace RazerSdkReader.Extensions;

internal static class EventWaitHandleHelper
{
    public static EventWaitHandle Create(string name)
    {
        return EventWaitHandleAcl.Create(false, EventResetMode.ManualReset, name, out var _, GetSecurity());
    }

    private static EventWaitHandleSecurity GetSecurity()
    {
        var security = new EventWaitHandleSecurity();
        var everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
        security.AddAccessRule(new EventWaitHandleAccessRule(everyone, EventWaitHandleRights.FullControl, AccessControlType.Allow));
        return security;
    }

    public static EventWaitHandle Open(string name)
    {
        return EventWaitHandle.OpenExisting(name);
    }

    public static void Pulse(string name)
    {
        var e = Open(name);
        e.Pulse();
        e.Close();
    }
}