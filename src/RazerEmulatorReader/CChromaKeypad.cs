using System.Runtime.InteropServices;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaKeypad
{ 
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint WriteIndex;
    
    private readonly uint Padding;
    
    public readonly CChromaKeypadData10 Data;
    
    public readonly CChromaDevice10 Device;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaKeypadData
{
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint Flag;
    
    [MarshalAs(UnmanagedType.I4)]
    public readonly int EffectType;
    
    [MarshalAs(UnmanagedType.Struct)]
    public readonly KeypadEffect Effect;
    
    [MarshalAs(UnmanagedType.U8)]
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

[UnmanagedArray(typeof(Color), 20)]
public readonly partial struct KeypadCustom
{

}

[UnmanagedArray(typeof(CChromaKeypadData), 10)]
public readonly partial struct CChromaKeypadData10
{

}
