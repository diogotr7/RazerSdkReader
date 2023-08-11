using System.Runtime.InteropServices;

namespace RazerEmulatorReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaHeadset
{
    public readonly CChromaHeadsetData10 Data;
    public readonly CChromaDevice10 Device;
    public readonly int Padding;
    public readonly int WriteIndex;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaHeadsetData
{
    public readonly HeadsetEffect Effect;
    public readonly int EffectType;
    public readonly uint Flag;
    public readonly uint Padding;
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct HeadsetEffect
{
    public readonly Breathing Breathing;
    public readonly HeadsetCustom Custom;
    public readonly Static Static;
}

[UnmanagedArray(typeof(CChromaColor), 5)]
public readonly partial record struct HeadsetCustom;

[UnmanagedArray(typeof(CChromaHeadsetData), 10)]
public readonly partial record struct CChromaHeadsetData10;