using System.Runtime.InteropServices;
using RazerEmulatorReader.Enums;

namespace RazerEmulatorReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaKeyboard
{
    public readonly CChromaKeyboardData10 Data;
    public readonly CChromaDevice10 Device;
    public readonly int Padding;
    public readonly int WriteIndex;
}

[UnmanagedArray(typeof(CChromaKeyboardData), 10)]
public readonly partial record struct CChromaKeyboardData10;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaKeyboardData
{
    public readonly KeyboardEffect Effect;
    public readonly EffectType EffectType;
    public readonly uint Flag;
    public readonly uint Padding;
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct KeyboardEffect
{
    public readonly Breathing Breathing;
    public readonly KeyboardCustom Custom;
    public readonly KeyboardCustom2 Custom2;
    public readonly KeyboardCustom3 Custom3;
    public readonly Reactive Reactive;
    public readonly Starlight Starlight;
    public readonly Static Static;
    public readonly Wave Wave;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct KeyboardCustom
{
    public readonly Color6X22 Color;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct KeyboardCustom2
{
    public readonly Color6X22 Color;
    public readonly Color6X22 Key;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct KeyboardCustom3
{
    public readonly Color8X24 Color;
    public readonly Color6X22 Key;
}

[UnmanagedArray(typeof(CChromaColor), 192)]
public readonly partial record struct Color8X24;

[UnmanagedArray(typeof(CChromaColor), 132)]
public readonly partial record struct Color6X22;