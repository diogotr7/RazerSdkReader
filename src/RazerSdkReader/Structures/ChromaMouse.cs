using RazerSdkReader.Enums;
using RazerSdkReader.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaMouse : IColorProvider
{
    private const int WIDTH = 7;
    private const int HEIGHT = 9;
    private const int COUNT = WIDTH * HEIGHT;

    public readonly uint WriteIndex;
    private readonly uint Padding;
    public readonly ChromaMouseData10 Data;
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
public readonly record struct ChromaMouseData
{
    public readonly uint Flag;
    public readonly MouseLedType Led;
    public readonly EffectType EffectType;
    public readonly MouseEffect Effect;
    public readonly ChromaTimestamp Timestamp;
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
