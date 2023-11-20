using RazerSdkReader.Enums;
using RazerSdkReader.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaMousepad : IColorProvider
{
    private const int WIDTH = 20;
    private const int HEIGHT = 1;
    private const int COUNT = WIDTH * HEIGHT;

    public readonly uint WriteIndex;
    private readonly uint Padding;
    public readonly ChromaMousepadData10 Data;
    public readonly ChromaDevice10 Device;

    public int Width => WIDTH;

    public int Height => HEIGHT;

    public int Count => COUNT;

    public ChromaColor GetColor(int index)
    {
        if (index < 0 || index >= COUNT)
            throw new ArgumentOutOfRangeException(nameof(index));

        ref readonly var data = ref Data[WriteIndex.ToReadIndex()];

        return ChromaEncryption.Decrypt(data.Effect.Custom2[index], data.Timestamp);
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaMousepadData
{
    public readonly uint Flag;
    public readonly EffectType EffectType;
    public readonly MousepadEffect Effect;
    public readonly ChromaTimestamp Timestamp;
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
