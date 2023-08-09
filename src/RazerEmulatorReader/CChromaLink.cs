using System.Runtime.InteropServices;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaLink
{ 
    public readonly uint WriteIndex;
    public readonly uint Padding;
    public readonly CChromaLinkData10 Data;
    public readonly CChromaDevice10 Device;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaLinkData
{
    public readonly uint Flag;
    public readonly int EffectType;
    public readonly LinkEffect Effect;
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct LinkEffect
{
    public readonly Breathing Breathing;
    public readonly LinkCustom Custom;
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

