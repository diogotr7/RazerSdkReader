using System.Runtime.InteropServices;

namespace RazerEmulatorReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CAppManager
{
    public readonly CAppManagerData10 Data;
    public readonly uint Padding;
    public readonly uint WriteIndex;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CAppManagerData
{
    public readonly uint ProcessId;
    public readonly int Register;
    public readonly ulong Timestamp;
}

[UnmanagedArray(typeof(CAppManagerData), 10)]
public readonly partial record struct CAppManagerData10;