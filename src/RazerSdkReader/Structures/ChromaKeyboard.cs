using RazerSdkReader.Enums;
using RazerSdkReader.Extensions;
using System;
using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaKeyboard : IColorProvider
{
    public const int WIDTH = 22;
    public const int HEIGHT = 6;
    public const int COUNT = WIDTH * HEIGHT;

    public readonly uint WriteIndex;
    private readonly int Padding;
    public readonly ChromaKeyboardData10 Data;
    public readonly ChromaDevice10 Device;

    public int Width => WIDTH;

    public int Height => HEIGHT;

    public int Count => COUNT;
    
    public ChromaColor GetColor(int index)
    {
        if (index is < 0 or >= COUNT)
            throw new ArgumentOutOfRangeException(nameof(index));

        ref readonly var data = ref Data[WriteIndex.ToReadIndex()];

        return ChromaEncryption.Decrypt(data.Effect.Custom.Color[index], data.Timestamp);
    }

    public void GetColors(Span<ChromaColor> colors)
    {
        ref readonly var data = ref Data[WriteIndex.ToReadIndex()];

        ChromaEncryption.Decrypt(data.Effect.Custom.Color, colors, data.Timestamp);
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaKeyboardData
{
    public readonly uint Flag;
    public readonly EffectType EffectType;
    public readonly KeyboardEffect Effect;
    private readonly uint Padding;
    public readonly ChromaTimestamp Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct KeyboardEffect
{
    public readonly Wave Wave;
    public readonly Breathing Breathing;
    public readonly Reactive Reactive;
    public readonly Starlight Starlight;
    public readonly Static Static;
    public readonly KeyboardCustom Custom;
    public readonly KeyboardCustom2 Custom2;
    public readonly KeyboardCustom3 Custom3;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct KeyboardCustom
{
    public readonly Color6X22 Color;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct KeyboardCustom2
{
    public readonly Color6X22 Color;
    public readonly Color6X22 Key;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct KeyboardCustom3
{
    public readonly Color8X24 Color;
    public readonly Color6X22 Key;
}