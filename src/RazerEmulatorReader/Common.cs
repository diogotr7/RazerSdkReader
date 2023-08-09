using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Breathing
{
    public readonly uint dwParam;
    public readonly int BreathingType;
    public readonly Color Color1;
    public readonly Color Color2;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Static
{ 
    public readonly uint dwParam;
    public readonly Color Color;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Color
{
    public readonly byte A;
    public readonly byte R;
    public readonly byte G;
    public readonly byte B;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Blinking
{
    public readonly uint dwParam;
    public readonly Color Color;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct None
{
    public readonly uint dwParam;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Reactive
{
    public readonly uint dwParam;
    public readonly Color Color;
    public readonly int Duration;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct SpectrumCycling
{
    public readonly uint dwParam;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Starlight
{
    public readonly uint dwParam;
    public readonly int Type;
    public readonly int Duration;
    public readonly Color Color1;
    public readonly Color Color2;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Wave
{
    public readonly uint dwParam;
    public readonly int Direction;
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

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaDevice
{
    public readonly Wchar260 Instance;
    public readonly Wchar260 InstanceName;
}

[UnmanagedArray(typeof(CChromaDevice), 10)]
public readonly partial struct CChromaDevice10
{
    
}