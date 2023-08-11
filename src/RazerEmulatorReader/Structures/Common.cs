using System.Runtime.InteropServices;
using RazerEmulatorReader.Enums;

namespace RazerEmulatorReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct Breathing
{
    public readonly uint dwParam;
    public readonly BreathingEffectType BreathingType;
    public readonly CChromaColor Color1;
    public readonly CChromaColor Color2;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct Static
{
    public readonly uint dwParam;
    public readonly CChromaColor Color;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct Blinking
{
    public readonly uint dwParam;
    public readonly CChromaColor Color;
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
    public readonly CChromaColor Color;
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
    public readonly CChromaColor Color1;
    public readonly CChromaColor Color2;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct Wave
{
    public readonly uint dwParam;
    public readonly EffectDirection Direction;
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