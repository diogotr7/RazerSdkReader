using RazerSdkReader.Enums;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct Breathing
{
    public readonly uint dwParam;
    public readonly BreathingEffectType BreathingType;
    public readonly ChromaColor Color1;
    public readonly ChromaColor Color2;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct Static
{
    public readonly uint dwParam;
    public readonly ChromaColor Color;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct Blinking
{
    public readonly uint dwParam;
    public readonly ChromaColor Color;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct None
{
    public readonly uint dwParam;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct Reactive
{
    public readonly uint dwParam;
    public readonly ChromaColor Color;
    public readonly EffectDuration Duration;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct SpectrumCycling
{
    public readonly uint dwParam;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct Starlight
{
    public readonly uint dwParam;
    public readonly BreathingEffectType Type;
    public readonly EffectDuration Duration;
    public readonly ChromaColor Color1;
    public readonly ChromaColor Color2;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct Wave
{
    public readonly uint dwParam;
    public readonly EffectDirection Direction;
    public readonly int Speed;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaDevice
{
    public readonly ChromaString Instance;
    public readonly ChromaString InstanceName;
}

[InlineArray(10)]
public struct ChromaDevice10
{
    public ChromaDevice _field;
}