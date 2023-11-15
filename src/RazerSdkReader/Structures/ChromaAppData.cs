using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaAppData
{
    //Note: This can be interpreted as 51 ChromaAppData structs,
    // but the first one seems to only contain the PID of the current app, with the name being empty.

    //I'm not sure what the padding here or in ChromaAppInfo is.

    public readonly uint AppCount;
    private readonly ChromaString UnusedAppName;
    public readonly uint CurrentAppId;
    private readonly uint Padding;

    public readonly ChromaAppInfo50 AppInfo;

    public string CurrentAppName
    {
        get
        {
            foreach (ref readonly var app in AppInfo)
            {
                if (CurrentAppId != app.AppId) continue;

                return app.AppName;
            }

            return string.Empty;
        }
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaAppInfo
{
    public readonly ChromaString AppName;
    public readonly uint AppId;
    private readonly uint Padding;
}

[InlineArray(50)]
public struct ChromaAppInfo50
{
    public ChromaAppInfo _field;
}