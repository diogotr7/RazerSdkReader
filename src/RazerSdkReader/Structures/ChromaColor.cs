using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaColor(byte R, byte G, byte B, byte A)
{
    //Note: DO NOT change the order of these fields.
    //They are ordered this way for compatibility
    //with SkiaSharp since that's what I'll use it with. (BGRA, little endian)
    //The sdk uses a different format, but we'll guarantee any 
    //colors the consumer gets from us are in the correct format.
    
    //if any uint is treated as a color in this project,
    //it can be assumed to be in the format of (ABGR, little endian)
    public readonly byte B = B;
    public readonly byte G = G;
    public readonly byte R = R;
    public readonly byte A = A;

    internal static ChromaColor FromSdkColor(uint color)
    {
        //sdk uses RGBA (little endian)
        var r = (byte)color;
        var g = (byte)(color >> 8);
        var b = (byte)(color >> 16);
        var a = (byte)(color >> 24);
        
        return new ChromaColor(r, g, b, a);
    }
}