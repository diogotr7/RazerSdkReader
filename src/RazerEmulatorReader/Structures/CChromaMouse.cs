using System.Runtime.InteropServices;

namespace RazerEmulatorReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaMouse
{
    public readonly CChromaMouseData10 Data;
    public readonly CChromaDevice10 Device;
    public readonly uint Padding;
    public readonly uint WriteIndex;
}

[UnmanagedArray(typeof(CChromaMouseData), 10)]
public readonly partial record struct CChromaMouseData10;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaMouseData
{
    public readonly MouseEffect Effect;
    public readonly int EffectType;
    public readonly uint Flag;
    public readonly uint Led;
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct MouseEffect
{
    public readonly Blinking Blinking;
    public readonly Breathing Breathing;
    public readonly MouseCustom Custom;
    public readonly MouseCustom2 Custom2;
    public readonly Reactive Reactive;
    public readonly Static Static;
    public readonly Wave Wave;
}

[UnmanagedArray(typeof(CChromaColor), 30)]
public readonly partial record struct MouseCustom;

[UnmanagedArray(typeof(CChromaColor), 63)]
public readonly partial record struct MouseCustom2;