using System.Runtime.InteropServices;

namespace RazerEmulatorReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaLink
{
    public readonly CChromaLinkData10 Data;
    public readonly CChromaDevice10 Device;
    public readonly uint Padding;
    public readonly uint WriteIndex;
}

[UnmanagedArray(typeof(CChromaLinkData), 10)]
public readonly partial record struct CChromaLinkData10;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaLinkData
{
    public readonly LinkEffect Effect;
    public readonly int EffectType;
    public readonly uint Flag;
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