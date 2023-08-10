using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaColor
{
    public readonly byte R;
    public readonly byte G;
    public readonly byte B;
    public readonly byte A;

    private CChromaColor(byte r, byte g, byte b, byte a)
    {
        
        R = r;
        G = g;
        B = b;
        A = a;
    }
    
    public static CChromaColor FromArgb(byte a, byte r, byte g, byte b)
    {
        return new CChromaColor(r, g, b, a);
    }

    public override string ToString()
    {
        return $"A: {A}, R: {R}, G: {G}, B: {B}";
    }
}