using System.Runtime.InteropServices;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaMouse
{
    public readonly uint WriteIndex;
    public readonly uint Padding;
    public readonly CChromaMouseData10 Data;
    public readonly CChromaDevice10 Device;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaMouseData
{
    public readonly uint Flag;
    public readonly uint Led;
    public readonly int EffectType;
    public readonly MouseEffect Effect;
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