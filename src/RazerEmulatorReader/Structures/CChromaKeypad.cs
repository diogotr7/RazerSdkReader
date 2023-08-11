using System.Runtime.InteropServices;

namespace RazerEmulatorReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaKeypad
{ 
    public readonly uint WriteIndex;
    public readonly uint Padding;
    public readonly CChromaKeypadData10 Data;
    public readonly CChromaDevice10 Device;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaKeypadData
{
    public readonly uint Flag;
    public readonly int EffectType;
    public readonly KeypadEffect Effect;
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct KeypadEffect
{
    public readonly Breathing Breathing;
    public readonly KeypadCustom Custom;
    public readonly Reactive Reactive;
    public readonly Static Static;
    public readonly Wave Wave;
}

[UnmanagedArray(typeof(CChromaColor), 20)]
public readonly partial record struct KeypadCustom;

[UnmanagedArray(typeof(CChromaKeypadData), 10)]
public readonly partial record struct CChromaKeypadData10;