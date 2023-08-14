using System.Runtime.InteropServices;
using RazerSdkReader.Enums;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaMouse
{
    public readonly int WriteIndex;
    public readonly uint Padding;
    public readonly ChromaMouseData10 Data;
    public readonly ChromaDevice10 Device;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaMouseData
{
    public readonly uint Flag;
    public readonly uint Led;
    public readonly EffectType EffectType;
    public readonly MouseEffect Effect;
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct MouseEffect
{
    public readonly Breathing Breathing;
    public readonly Blinking Blinking;
    public readonly MouseCustom Custom;
    public readonly MouseCustom2 Custom2;
    public readonly Reactive Reactive;
    public readonly Static Static;
    public readonly Wave Wave;
}

[UnmanagedArray(typeof(ChromaColor), 30)]
public readonly partial record struct MouseCustom;

[UnmanagedArray(typeof(ChromaColor), 63)]
public readonly partial record struct MouseCustom2;

[UnmanagedArray(typeof(ChromaMouseData), 10)]
public readonly partial record struct ChromaMouseData10;