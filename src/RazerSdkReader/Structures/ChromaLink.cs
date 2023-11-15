using RazerSdkReader.Enums;
using RazerSdkReader.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaLink : IColorProvider
{
    public readonly uint WriteIndex;
    private readonly uint Padding;
    public readonly ChromaLinkData10 Data;
    public readonly ChromaDevice10 Device;

    public int Width => 5;

    public int Height => 1;

    public int Count => Width * Height;

    public readonly ChromaColor GetColor(int index)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        var targetIndex = WriteIndex.ToReadIndex();
        var snapshot = Data[targetIndex];

        if (snapshot.EffectType is not EffectType.Custom and not EffectType.Static)
            return default;

        var clr = snapshot.Effect.Custom[index];
        var staticColor = snapshot.Effect.Static.Color;

        return clr ^ staticColor;
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaLinkData
{
    public readonly uint Flag;
    public readonly EffectType EffectType;
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

[InlineArray(50)]
public struct LinkCustom
{
    public ChromaColor _field;
}

[InlineArray(10)]
public struct ChromaLinkData10
{
    public ChromaLinkData _field;
}