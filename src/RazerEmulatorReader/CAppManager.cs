using System.Runtime.InteropServices;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly  struct CAppManagerData
{
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint ProcessId;

    [MarshalAs(UnmanagedType.I4)]
    public readonly int Register;

    [MarshalAs(UnmanagedType.U8)]
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CAppManager
{
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint WriteIndex;
    
    [MarshalAs(UnmanagedType.U4)]
    private readonly uint Padding;

    public readonly CAppManager10 Data;
}

[UnmanagedArray(typeof(CAppManagerData), 10)]
public readonly partial struct CAppManager10
{ }
