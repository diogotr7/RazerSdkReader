using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

//unused, information largely useless. ChromaAppData is more useful.
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaAppManager
{
    public readonly uint WriteIndex;
    private readonly uint Padding;
    public readonly ChromaAppManager10 Data;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaAppManagerData
{
    public readonly uint ProcessId;
    public readonly int Register;
    public readonly ulong Timestamp;
}

[InlineArray(10)]
public struct ChromaAppManager10
{
    public ChromaAppManagerData _field;
}