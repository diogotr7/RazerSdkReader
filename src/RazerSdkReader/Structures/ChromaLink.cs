using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaLink
{ 
    public readonly int WriteIndex;
    public readonly uint Padding;
    public readonly ChromaLinkData10 Data;
    public readonly ChromaDevice10 Device;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaLinkData
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

[UnmanagedArray(typeof(ChromaColor), 50)]
public readonly partial record struct LinkCustom;

[UnmanagedArray(typeof(ChromaLinkData), 10)]
public readonly partial record struct ChromaLinkData10;