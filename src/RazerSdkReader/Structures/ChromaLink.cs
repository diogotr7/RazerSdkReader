using RazerSdkReader.Enums;
using RazerSdkReader.Extensions;
using System;
using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaLink : IColorProvider
{
    private const int WIDTH = 5;
    private const int HEIGHT = 1;//technically 10, but only 5 are ever used.
    private const int COUNT = WIDTH * HEIGHT;

    public readonly uint WriteIndex;
    private readonly uint Padding;
    public readonly ChromaLinkData10 Data;
    public readonly ChromaDevice10 Device;

    public int Width => WIDTH;

    public int Height => HEIGHT;

    public int Count => COUNT;

    public ChromaColor GetColor(int index)
    {
        if (index is < 0 or >= COUNT)
            throw new ArgumentOutOfRangeException(nameof(index));

        ref readonly var data = ref Data[WriteIndex.ToReadIndex()];

        return ChromaEncryption.Decrypt(data.Effect.Custom[index], data.Timestamp);
    }

    public void GetColors(Span<ChromaColor> colors)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(colors.Length, COUNT);

        ref readonly var data = ref Data[WriteIndex.ToReadIndex()];

        ChromaEncryption.Decrypt(data.Effect.Custom, colors, data.Timestamp);
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaLinkData
{
    public readonly uint Flag;
    public readonly EffectType EffectType;
    public readonly LinkEffect Effect;
    public readonly ChromaTimestamp Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct LinkEffect
{
    public readonly Breathing Breathing;
    public readonly LinkCustom Custom;
    public readonly Static Static;
}