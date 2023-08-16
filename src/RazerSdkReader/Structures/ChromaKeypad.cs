using System;
using System.Runtime.InteropServices;
using RazerSdkReader.Enums;
using RazerSdkReader.Extensions;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaKeypad : IColorProvider
{ 
    public readonly int WriteIndex;
    public readonly uint Padding;
    public readonly ChromaKeypadData10 Data;
    public readonly ChromaDevice10 Device;
    
    public int Width => 5;
    
    public int Height => 4;
    
    public int Count => Width * Height;
    
    public ChromaColor GetColor(int index, int? frame = null)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        
        var targetIndex = frame ?? WriteIndex.ToReadIndex();
        var snapshot = Data[targetIndex];
        
        if (snapshot.EffectType is not EffectType.Custom and not EffectType.Static)
            return default;
        
        var clr =  snapshot.Effect.Custom[index];
        var staticColor = snapshot.Effect.Static.Color;
        
        return clr ^ staticColor;
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaKeypadData
{
    public readonly uint Flag;
    public readonly EffectType EffectType;
    public readonly KeypadEffect Effect;
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct KeypadEffect
{
    public readonly Breathing Breathing;
    public readonly KeypadCustom Custom;
    public readonly Reactive Reactive;
    public readonly Static Static;
    public readonly Wave Wave;
}

[UnmanagedArray(typeof(ChromaColor), 20)]
public readonly partial record struct KeypadCustom;

[UnmanagedArray(typeof(ChromaKeypadData), 10)]
public readonly partial record struct ChromaKeypadData10;