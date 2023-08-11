using System.Runtime.InteropServices;

namespace RazerEmulatorReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CAppData
{
    //Note: This can be interpreted as 51 AppData structs,
    // but the first one seems to only contain the PID of the current app, with the name being empty.

    //I'm not sure what the padding here or in AppInfo is.

    public readonly uint AppCount;

    public readonly AppInfo50 AppInfo;
    public readonly uint CurrentAppId;
    public readonly uint Padding;
    public readonly CChromaString PaddingChars;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct AppInfo
{
    public readonly uint AppId;
    public readonly CChromaString AppName;
    public readonly uint Padding;
}

[UnmanagedArray(typeof(AppInfo), 50)]
public readonly partial record struct AppInfo50;