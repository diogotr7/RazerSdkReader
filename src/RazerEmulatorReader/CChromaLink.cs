using System.Runtime.InteropServices;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaLink
{ 
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint WriteIndex;
    
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint Padding;
    
    public readonly CChromaLinkData10 Data;
    
    public readonly CChromaDevice10 Device;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaLinkData
{
    [MarshalAs(UnmanagedType.U4)]
    public readonly uint Flag;
    
    [MarshalAs(UnmanagedType.I4)]
    public readonly int EffectType;

    [MarshalAs(UnmanagedType.Struct)]
    public readonly LinkEffect Effect;
    
    [MarshalAs(UnmanagedType.U8)]
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct LinkEffect
{
    [MarshalAs(UnmanagedType.Struct)]
    public readonly Breathing Breathing;
    
    [MarshalAs(UnmanagedType.Struct)]
    public readonly LinkCustom Custom;
    
    [MarshalAs(UnmanagedType.Struct)]
    public readonly Static Static;
}

[UnmanagedArray(typeof(Color), 50)]
public readonly partial struct LinkCustom
{

}

[UnmanagedArray(typeof(CChromaLinkData), 10)]
public readonly partial struct CChromaLinkData10
{

}

