using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace RazerSdkReader.Extensions;

internal static class EventWaitHandleHelper
{
    [SupportedOSPlatform("windows")]
    public static EventWaitHandle Create(string name)
    {
        if (EventWaitHandleAcl.TryOpenExisting(name, EventWaitHandleRights.Modify | EventWaitHandleRights.Synchronize, out var existingHandle))
        {
            return existingHandle;
        }

        return EventWaitHandleAcl.Create(false, EventResetMode.ManualReset, name, out _, CreateEventWaitHandleSecurity());
    }

    [SupportedOSPlatform("windows")]
    private static EventWaitHandleSecurity CreateEventWaitHandleSecurity(EventWaitHandleRights rights = EventWaitHandleRights.FullControl | EventWaitHandleRights.TakeOwnership)
    {
        var security = new EventWaitHandleSecurity();
        var identifier = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
        security.AddAccessRule(new EventWaitHandleAccessRule(identifier, rights, AccessControlType.Allow));
        return security;
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