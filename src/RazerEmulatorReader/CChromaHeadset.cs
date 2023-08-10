using System.Runtime.InteropServices;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct HeadsetEffect
{
   public readonly Breathing Breathing;
   public readonly HeadsetCustom Custom;
   public readonly Static Static;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaHeadsetData
{
    public readonly uint Flag;
    public readonly int EffectType;
    public readonly HeadsetEffect Effect;
    public readonly uint Padding;
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaHeadset
{
    public readonly int WriteIndex;
    public readonly int Padding;
    public readonly CChromaHeadsetData10 Data;
    public readonly CChromaDevice10 Device;
}

[UnmanagedArray(typeof(CChromaColor), 5)]
public readonly partial record struct HeadsetCustom
{
    
}

[UnmanagedArray(typeof(CChromaHeadsetData), 10)]
public readonly partial record struct CChromaHeadsetData10
{
    
}

