using RazerSdkReader.Enums;
using RazerSdkReader.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaKeypad : IColorProvider
{
    public const int WIDTH = 5;
    public const int HEIGHT = 4;
    public const int COUNT = WIDTH * HEIGHT;

    public readonly uint WriteIndex;
    private readonly uint Padding;
    public readonly Array10<ChromaKeypadData> Data;
    public readonly Array10<ChromaDevice> Device;

    public int Width => WIDTH;

    public int Height => HEIGHT;

    public int Count => COUNT;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ChromaColor GetColor(int index)
    {
        if (index is < 0 or >= COUNT)
            throw new ArgumentOutOfRangeException(nameof(index));

        ref readonly var data = ref Data[WriteIndex.ToReadIndex()];

        return data.EffectType switch
        {
            EffectType.Static => ChromaEncryption.Decrypt(data.Effect.Static.Color, data.Timestamp),
            _ => ChromaEncryption.Decrypt(data.Effect.Custom[index], data.Timestamp),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void GetColors(Span<ChromaColor> colors)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(colors.Length, COUNT);

        ref readonly var data = ref Data[WriteIndex.ToReadIndex()];

        if (data.EffectType == EffectType.Static)
        {
            var color = ChromaEncryption.Decrypt(data.Effect.Static.Color, data.Timestamp);
            colors.Fill(color);
            return;
        }
        
        ChromaEncryption.Decrypt(data.Effect.Custom, colors, data.Timestamp);
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaKeypadData
{
    public readonly uint Flag;
    public readonly EffectType EffectType;
    public readonly KeypadEffect Effect;
    public readonly ChromaTimestamp Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct KeypadEffect
{
    public readonly Breathing Breathing;
    public readonly Array20<uint> Custom;
    public readonly Reactive Reactive;
    public readonly Static Static;
    public readonly Wave Wave;
}