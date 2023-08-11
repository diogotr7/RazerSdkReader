using System.Runtime.InteropServices;
using RazerEmulatorReader.Enums;

namespace RazerEmulatorReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct Breathing
{
    public readonly BreathingEffectType BreathingType;
    public readonly CChromaColor Color1;
    public readonly CChromaColor Color2;
    public readonly uint dwParam;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct Static
{
    public readonly CChromaColor Color;
    public readonly uint dwParam;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct Blinking
{
    public readonly CChromaColor Color;
    public readonly uint dwParam;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct None
{
    public readonly uint dwParam;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct Reactive
{
    public readonly CChromaColor Color;
    public readonly EffectDuration Duration;
    public readonly uint dwParam;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct SpectrumCycling
{
    public readonly uint dwParam;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct Starlight
{
    public readonly CChromaColor Color1;
    public readonly CChromaColor Color2;
    public readonly EffectDuration Duration;
    public readonly uint dwParam;
    public readonly BreathingEffectType Type;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct Wave
{
    public readonly EffectDirection Direction;
    public readonly uint dwParam;
    public readonly int Speed;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaDevice
{
    public readonly CChromaString Instance;
    public readonly CChromaString InstanceName;
}

[UnmanagedArray(typeof(CChromaDevice), 10)]
public readonly partial record struct CChromaDevice10;