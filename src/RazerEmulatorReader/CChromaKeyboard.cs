using System;
using System.Drawing;
using System.Runtime.InteropServices;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaKeyboard
{
    [MarshalAs(UnmanagedType.I4)] public readonly int WriteIndex;

    [MarshalAs(UnmanagedType.I4)] public readonly int Padding;

    public readonly CChromaKeyboardData10 Data;

    public readonly CChromaDevice10 Device;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaKeyboardData
{
    [MarshalAs(UnmanagedType.U4)] public readonly uint Flag;

    [MarshalAs(UnmanagedType.I4)] public readonly int EffectType;

    [MarshalAs(UnmanagedType.Struct)] public readonly KeyboardEffect Effect;

    [MarshalAs(UnmanagedType.U4)] public readonly uint Padding;

    [MarshalAs(UnmanagedType.U8)] public readonly ulong Timestamp;
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

    //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 356)]
    //public readonly byte[] Padding;

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
public readonly partial struct Key6X22 { }

[UnmanagedArray(typeof(Color), 192)]
public readonly partial struct Color8X24 { }

[UnmanagedArray(typeof(Color), 132)]
public readonly partial struct Color6X22 { }

[UnmanagedArray(typeof(CChromaKeyboardData), 10)]
public readonly partial struct CChromaKeyboardData10
{
}