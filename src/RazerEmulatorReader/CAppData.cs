using System.Runtime.InteropServices;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CAppData
{
    //Note: This can be interpreted as 51 AppData structs,
    // but the first one seems to only contain the PID of the current app, with the name being empty.
    
    //I'm not sure what the padding here or in AppInfo is.
    
    public readonly uint AppCount;
    private readonly Wchar260 Padding4;
    public readonly uint CurrentAppId;
    private readonly uint Padding;
    
    public readonly AppInfo50 AppInfo;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct AppInfo
{
    public readonly Wchar260 AppName;
    public readonly uint AppId;
    public readonly uint Padding;
}

[UnmanagedArray(typeof(AppInfo), 50)]
public readonly partial struct AppInfo50 { }