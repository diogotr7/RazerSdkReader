using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace RazerSdkReader.Extensions;

internal static class MutexHelper
{
    public static Mutex CreateMutex(string name)
    {
        return MutexAcl.Create(false, name, out var _, GetSecurity());
    }

    private static MutexSecurity GetSecurity()
    {
        var security = new MutexSecurity();
        var everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
        security.AddAccessRule(new MutexAccessRule(everyone, MutexRights.FullControl, AccessControlType.Allow));
        return security;
    }
}