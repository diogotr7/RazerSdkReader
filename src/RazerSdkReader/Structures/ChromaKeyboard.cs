using RazerSdkReader.Enums;
using RazerSdkReader.Extensions;
using System;
using System.Runtime.CompilerServices;
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
    public readonly Array10<ChromaKeyboardData> Data;
    public readonly Array10<ChromaDevice> Device;

    public int Width => WIDTH;

    public int Height => HEIGHT;

    public int Count => COUNT;

    public ChromaColor GetColor(int index)
    {
        if (index is < 0 or >= COUNT)
            throw new ArgumentOutOfRangeException(nameof(index));

        ref readonly var data = ref Data[WriteIndex.ToReadIndex()];

        var x = data.EffectType switch
        {
            EffectType.Static => ChromaEncryption.Decrypt(data.Effect.Static.Color, data.Timestamp),
            EffectType.CustomKey => ChromaEncryption.Decrypt(data.Effect.Custom2.Color[index], data.Timestamp),
            _ => ChromaEncryption.Decrypt(data.Effect.Custom[index], data.Timestamp)
        };

        return Unsafe.As<uint, ChromaColor>(ref x);
    }

    public void GetColors(Span<ChromaColor> colors)
    {
        var casted = MemoryMarshal.Cast<ChromaColor, uint>(colors);
        ref readonly var data = ref Data[WriteIndex.ToReadIndex()];

        if (data.EffectType == EffectType.Static)
        {
            var color = ChromaEncryption.Decrypt(data.Effect.Static.Color, data.Timestamp);
            casted.Fill(color);
        }
        else if (data.EffectType == EffectType.CustomKey)
        {
            ChromaEncryption.Decrypt(data.Effect.Custom2.Color, casted, data.Timestamp);
        }
        else
        {
            ChromaEncryption.Decrypt(data.Effect.Custom, casted, data.Timestamp);
        }
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
    public readonly Array132<uint> Custom;
    public readonly KeyboardCustom2 Custom2;
    public readonly KeyboardCustom3 Custom3;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct KeyboardCustom2
{
    public readonly Array132<uint> Color;
    public readonly Array132<uint> Key;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct KeyboardCustom3
{
    public readonly Array192<uint> Color;
    public readonly Array132<uint> Key;
}