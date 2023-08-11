using System.Runtime.InteropServices;

namespace RazerEmulatorReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaColor
{
    public readonly byte A;
    public readonly byte R;
    public readonly byte G;
    public readonly byte B;
    
    private CChromaColor(byte a, byte r, byte g, byte b)
    {
        A = a;
        R = r;
        G = g;
        B = b;
    }
    
    public static CChromaColor FromArgb(byte a, byte r, byte g, byte b) => new CChromaColor(a, r, g, b);
}