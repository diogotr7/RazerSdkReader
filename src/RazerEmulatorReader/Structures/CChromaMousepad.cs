using System.Runtime.InteropServices;

namespace RazerEmulatorReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaMousepad
{
    public readonly CChromaMousepadData10 Data;
    public readonly CChromaDevice10 Device;
    public readonly uint Padding;
    public readonly uint WriteIndex;
}

[UnmanagedArray(typeof(CChromaMousepadData), 10)]
public readonly partial record struct CChromaMousepadData10;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaMousepadData
{
    public readonly MousepadEffect Effect;
    public readonly int EffectType;
    public readonly uint Flag;
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct MousepadEffect
{
    public readonly Breathing Breathing;
    public readonly MousepadCustom Custom;
    public readonly MousepadCustom2 Custom2;
    public readonly Static Static;
    public readonly Wave Wave;
}

[UnmanagedArray(typeof(CChromaColor), 15)]
public readonly partial record struct MousepadCustom;

[UnmanagedArray(typeof(CChromaColor), 20)]
public readonly partial record struct MousepadCustom2;