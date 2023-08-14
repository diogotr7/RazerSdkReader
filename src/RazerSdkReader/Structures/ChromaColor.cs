using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaColor
{
    public readonly byte R;
    public readonly byte G;
    public readonly byte B;
    public readonly byte A;
    
    private ChromaColor(byte r, byte g, byte b, byte a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }
    
    public static ChromaColor operator ^(ChromaColor a, ChromaColor b) => new((byte)(a.R ^ b.R), (byte)(a.G ^ b.G), (byte)(a.B ^ b.B), (byte)(a.A ^ b.A));
}