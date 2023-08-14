using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaMousepad
{ 
    public readonly int WriteIndex;
    public readonly uint Padding;
    public readonly ChromaMousepadData10 Data;
    public readonly ChromaDevice10 Device;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaMousepadData
{
    public readonly uint Flag;
    public readonly int EffectType;
    public readonly MousepadEffect Effect;
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct MousepadEffect
{
    public readonly Breathing Breathing;
    public readonly Wave Wave;
    public readonly Static Static;
    public readonly MousepadCustom Custom;
    public readonly MousepadCustom2 Custom2;
}

[UnmanagedArray(typeof(ChromaColor), 15)]
public readonly partial record struct MousepadCustom;

[UnmanagedArray(typeof(ChromaColor), 20)]
public readonly partial record struct MousepadCustom2;

[UnmanagedArray(typeof(ChromaMousepadData), 10)]
public readonly partial record struct ChromaMousepadData10;