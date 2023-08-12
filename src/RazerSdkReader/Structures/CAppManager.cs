using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;


[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CAppManagerData
{
    public readonly uint ProcessId;
    public readonly int Register;
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CAppManager
{
    public readonly int WriteIndex;
    public readonly uint Padding;
    public readonly CAppManager10 Data;
}

[UnmanagedArray(typeof(CAppManagerData), 10)]
public readonly partial record struct CAppManager10;