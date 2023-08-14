using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaAppManager
{
    public readonly int WriteIndex;
    public readonly uint Padding;
    public readonly ChromaAppManager10 Data;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaAppManagerData
{
    public readonly uint ProcessId;
    public readonly int Register;
    public readonly ulong Timestamp;
}

[UnmanagedArray(typeof(ChromaAppManagerData), 10)]
public readonly partial record struct ChromaAppManager10;