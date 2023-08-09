using System.Runtime.InteropServices;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaMousepad
{ 
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint WriteIndex;
    
    private readonly uint Padding;
    
    public readonly CChromaMousepadData10 Data;
    
    public readonly CChromaDevice10 Device;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaMousepadData
{
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint Flag;
    
    [MarshalAs(UnmanagedType.I4)]
    public readonly int EffectType;
    
    [MarshalAs(UnmanagedType.Struct)]
    public readonly MousepadEffect Effect;
    
    [MarshalAs(UnmanagedType.U8)]
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct MousepadEffect
{
    public readonly Breathing Breathing;
    public readonly Wave Wave;
    public readonly Static Static;
    public readonly MousepadCustom Custom;
    public readonly MousepadCustom2 Custom2;
}

[UnmanagedArray(typeof(Color), 15)]
public readonly partial struct MousepadCustom
{

}

[UnmanagedArray(typeof(Color), 20)]
public readonly partial struct MousepadCustom2
{
}

[UnmanagedArray(typeof(CChromaMousepadData), 10)]
public readonly partial struct CChromaMousepadData10
{

}