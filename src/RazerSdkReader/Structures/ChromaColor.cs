using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaColor(byte R, byte G, byte B, byte A)
{
    public readonly byte R = R;
    public readonly byte G = G;
    public readonly byte B = B;
    public readonly byte A = A;

    public static ChromaColor operator ^(ChromaColor a, ChromaColor b) =>
        new((byte)(a.R ^ b.R), (byte)(a.G ^ b.G), (byte)(a.B ^ b.B), (byte)(a.A ^ b.A));
}