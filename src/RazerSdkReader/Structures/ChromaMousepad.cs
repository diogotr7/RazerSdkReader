using System;
using System.Runtime.InteropServices;
using RazerSdkReader.Enums;
using RazerSdkReader.Extensions;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaMousepad : IColorProvider
{
    public readonly int WriteIndex;
    public readonly uint Padding;
    public readonly ChromaMousepadData10 Data;
    public readonly ChromaDevice10 Device;

    public int Width => 20;

    public int Height => 1;

    public int Count => Width * Height;

    public ChromaColor GetColor(int index)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        var targetIndex = WriteIndex.ToReadIndex();
        var snapshot = Data[targetIndex];

        if (snapshot.EffectType is not EffectType.Custom and not EffectType.CustomKey and not EffectType.Static)
            return default;

        var clr = snapshot.Effect.Custom2[index];
        var staticColor = snapshot.Effect.Static.Color;

        return clr ^ staticColor;
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaMousepadData
{
    public readonly uint Flag;
    public readonly EffectType EffectType;
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