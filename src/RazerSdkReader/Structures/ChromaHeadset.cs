﻿using RazerSdkReader.Enums;
using RazerSdkReader.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaHeadset : IColorProvider
{
    public readonly uint WriteIndex;
    private readonly int Padding;
    public readonly ChromaHeadsetData10 Data;
    public readonly ChromaDevice10 Device;

    public int Width => 5;

    public int Height => 1;

    public int Count => Width * Height;

    public readonly ChromaColor GetColor(int index)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        ref readonly var data = ref Data[WriteIndex.ToReadIndex()];

        return ChromaEncryption.Decrypt(data.Effect.Custom[index], data.Timestamp);
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaHeadsetData
{
    public readonly uint Flag;
    public readonly EffectType EffectType;
    public readonly HeadsetEffect Effect;
    private readonly uint Padding;
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct HeadsetEffect
{
    public readonly Breathing Breathing;
    public readonly HeadsetCustom Custom;
    public readonly Static Static;
}