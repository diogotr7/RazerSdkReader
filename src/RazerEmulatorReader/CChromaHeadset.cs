using System;
using System.Runtime.InteropServices;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct HeadsetCustom
{
    public readonly Color5 Colors;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct HeadsetEffect
{
    [MarshalAs(UnmanagedType.Struct)] public readonly Breathing Breathing;
    [MarshalAs(UnmanagedType.Struct)] public readonly HeadsetCustom Custom;
    [MarshalAs(UnmanagedType.Struct)] public readonly Static Static;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaHeadsetData
{
    [MarshalAs(UnmanagedType.U4)] public readonly uint Flag;

    [MarshalAs(UnmanagedType.I4)] public readonly int EffectType;

    [MarshalAs(UnmanagedType.Struct)] public readonly HeadsetEffect Effect;

    [MarshalAs(UnmanagedType.U4)] public readonly uint Padding;
    
    [MarshalAs(UnmanagedType.U8)] public readonly ulong Timestamp;
    
    // [MarshalAs(UnmanagedType.U4)] public readonly uint Timestamp1;
    // [MarshalAs(UnmanagedType.U4)] public readonly uint Timestamp2;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaHeadset
{
    [MarshalAs(UnmanagedType.I4)] public readonly int WriteIndex;
    
    [MarshalAs(UnmanagedType.I4)] public readonly int Padding;

    public readonly CChromaHeadsetData10 Data;

    public readonly CChromaDevice10 Device;
}

[UnmanagedArray(typeof(Color), 5)]
public readonly partial struct Color5
{ }

[UnmanagedArray(typeof(CChromaHeadsetData), 10)]
public readonly partial struct CChromaHeadsetData10
{ }

