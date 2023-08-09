using System.Runtime.InteropServices;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaMouse
{
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint WriteIndex;

    private readonly uint Padding;
    
    public readonly CChromaMouseData10 Data;
    
    public readonly CChromaDevice10 Device;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaMouseData
{
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint Flag;
    
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint Led;
    
    [MarshalAs(UnmanagedType.I4)]
    public readonly int EffectType;
    
    [MarshalAs(UnmanagedType.Struct)]
    public readonly MouseEffect Effect;
    
    [MarshalAs(UnmanagedType.U8)]
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct MouseEffect
{
    public readonly Breathing Breathing;
    public readonly Blinking Blinking;
    public readonly MouseCustom Custom;
    public readonly MouseCustom2 Custom2;
    public readonly Reactive Reactive;
    public readonly Static Static;
    public readonly Wave Wave;
}

[UnmanagedArray(typeof(Color), 30)]
public readonly partial struct MouseCustom
{
}

[UnmanagedArray(typeof(Color), 63)]
public readonly partial struct MouseCustom2
{
}

[UnmanagedArray(typeof(CChromaMouseData), 10)]
public readonly partial struct CChromaMouseData10
{
}