using System.Runtime.InteropServices;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaKeypad
{ 
    public readonly uint WriteIndex;
    public readonly uint Padding;
    public readonly CChromaKeypadData10 Data;
    public readonly CChromaDevice10 Device;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaKeypadData
{
    public readonly uint Flag;
    public readonly int EffectType;
    public readonly KeypadEffect Effect;
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct KeypadEffect
{
    public readonly Breathing Breathing;
    public readonly KeypadCustom Custom;
    public readonly Reactive Reactive;
    public readonly Static Static;
    public readonly Wave Wave;
}

[UnmanagedArray(typeof(CChromaColor), 20)]
public readonly partial struct KeypadCustom
{

}

[UnmanagedArray(typeof(CChromaKeypadData), 10)]
public readonly partial struct CChromaKeypadData10
{

}
