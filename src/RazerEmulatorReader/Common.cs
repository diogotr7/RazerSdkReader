using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x10)]
public readonly struct Breathing
{
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint dwParam;
    
    [MarshalAs(UnmanagedType.I4)]
    public readonly int BreathingType;
    
    public readonly Color Color1;
    
    public readonly Color Color2;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x8)]
public readonly struct Static
{ 
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint dwParam;

    public readonly Color Color;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x4)]
public readonly struct Color
{
    [MarshalAs(UnmanagedType.U1)]
    public readonly byte A;
    
    [MarshalAs(UnmanagedType.U1)]
    public readonly byte R;
    
    [MarshalAs(UnmanagedType.U1)]
    public readonly byte G;
    
    [MarshalAs(UnmanagedType.U1)]
    public readonly byte B;

    public Color(uint color)
    {
        A = (byte) ((color >> 24) & 0xFF);
        R = (byte) ((color >> 16) & 0xFF);
        G = (byte) ((color >> 8) & 0xFF);
        B = (byte) (color & 0xFF);
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x8)]
public readonly struct Blinking
{
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint dwParam;
    
    public readonly Color Color;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x4)]
public readonly struct None
{
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint dwParam;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0xC)]
public readonly struct Reactive
{
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint dwParam;
    
    public readonly Color Color;
    
    [MarshalAs(UnmanagedType.I4)]
    public readonly int Duration;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x4)]
public readonly struct SpectrumCycling
{
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint dwParam;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x14)]
public readonly struct Starlight
{
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint dwParam;
    
    [MarshalAs(UnmanagedType.I4)]
    public readonly int Type;
    
    [MarshalAs(UnmanagedType.I4)]
    public readonly int Duration;
    
    public readonly Color Color1;
    
    public readonly Color Color2;
}
//oxc
[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0xC)]
public readonly struct Wave
{
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint dwParam;
    
    [MarshalAs(UnmanagedType.I4)]
    public readonly int Direction;
    
    [MarshalAs(UnmanagedType.I4)]
    public readonly int Speed;
}

[UnmanagedArray(typeof(char), 260)]
public readonly partial struct Wchar260
{
    public override string ToString()
    {
        var sb = new StringBuilder();

        for (var i = 0; i < Count; i++)
        {
            if (this[i] == 0)
                break;

            sb.Append(this[i]);
        }
        
        return sb.ToString();
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x410)]
public readonly struct CChromaDevice
{
    public readonly Wchar260 Instance;
    
    public readonly Wchar260 InstanceName;
}

[UnmanagedArray(typeof(CChromaDevice), 10)]
public readonly partial struct CChromaDevice10
{ }