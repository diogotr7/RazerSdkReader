using System.Runtime.InteropServices;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaKeyboard
{
    public readonly int WriteIndex;
    public readonly int Padding;
    public readonly CChromaKeyboardData10 Data;
    public readonly CChromaDevice10 Device;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaKeyboardData
{
    public readonly uint Flag;
    public readonly int EffectType;
    public readonly KeyboardEffect Effect;
    public readonly uint Padding;
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct KeyboardEffect
{
    public readonly Wave Wave;
    public readonly Breathing Breathing;
    public readonly Reactive Reactive;
    public readonly Starlight Starlight;
    public readonly Static Static;
    public readonly KeyboardCustom Custom;
    public readonly KeyboardCustom2 Custom2;
    public readonly KeyboardCustom3 Custom3;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct KeyboardCustom
{
    public readonly Color6X22 Color;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct KeyboardCustom2
{
    public readonly Color6X22 Color;
    public readonly Key6X22 Key;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct KeyboardCustom3
{
    public readonly Color8X24 Color;
    public readonly Key6X22 Key;
}

[UnmanagedArray(typeof(uint), 132)]
public readonly partial struct Key6X22
{
    
}

[UnmanagedArray(typeof(Color), 192)]
public readonly partial struct Color8X24
{
    
}

[UnmanagedArray(typeof(Color), 132)]
public readonly partial struct Color6X22
{
    
}

[UnmanagedArray(typeof(CChromaKeyboardData), 10)]
public readonly partial struct CChromaKeyboardData10
{
    
}