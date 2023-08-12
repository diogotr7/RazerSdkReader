using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaLink
{ 
    public readonly uint WriteIndex;
    public readonly uint Padding;
    public readonly CChromaLinkData10 Data;
    public readonly CChromaDevice10 Device;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaLinkData
{
    public readonly uint Flag;
    public readonly int EffectType;
    public readonly LinkEffect Effect;
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct LinkEffect
{
    public readonly Breathing Breathing;
    public readonly LinkCustom Custom;
    public readonly Static Static;
}

[UnmanagedArray(typeof(CChromaColor), 50)]
public readonly partial record struct LinkCustom;

[UnmanagedArray(typeof(CChromaLinkData), 10)]
public readonly partial record struct CChromaLinkData10;