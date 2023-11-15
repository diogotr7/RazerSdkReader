using RazerSdkReader.Enums;
using RazerSdkReader.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaMouse : IColorProvider
{
    public readonly uint WriteIndex;
    private readonly uint Padding;
    public readonly ChromaMouseData10 Data;
    public readonly ChromaDevice10 Device;

    public int Width => 7;

    public int Height => 9;

    public int Count => Width * Height;

    public readonly ChromaColor GetColor(int index)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        var targetIndex = WriteIndex.ToReadIndex();

        var snapshot = Data[targetIndex];

        if (
            snapshot.EffectType
            is not EffectType.Custom
                and not EffectType.CustomKey
                and not EffectType.Static
        )
            return default;

        var staticColor = snapshot.Effect.Static.Color;
        var clr = snapshot.Effect.Custom2[index];
        return clr ^ staticColor;
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaMouseData
{
    public readonly uint Flag;
    public readonly MouseLedType Led;
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

[InlineArray(30)]
public struct MouseCustom
{
    public ChromaColor _field;
}

[InlineArray(63)]
public struct MouseCustom2
{
    public ChromaColor _field;
}

[InlineArray(10)]
public struct ChromaMouseData10
{
    public ChromaMouseData _field;
}
