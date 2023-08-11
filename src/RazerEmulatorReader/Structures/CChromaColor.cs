using System.Runtime.InteropServices;

namespace RazerEmulatorReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaColor(byte R, byte G, byte B, byte A)
{
    public readonly byte A = A;
    public readonly byte B = B;
    public readonly byte G = G;
    public readonly byte R = R;
}