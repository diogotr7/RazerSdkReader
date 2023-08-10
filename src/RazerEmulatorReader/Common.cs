using System.Runtime.InteropServices;
using System.Text;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Breathing
{
    public readonly uint dwParam;
    public readonly BreathingEffectType BreathingType;
    public readonly CChromaColor Color1;
    public readonly CChromaColor Color2;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Static
{
    public readonly uint dwParam;
    public readonly CChromaColor Color;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Blinking
{
    public readonly uint dwParam;
    public readonly CChromaColor Color;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct None
{
    public readonly uint dwParam;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Reactive
{
    public readonly uint dwParam;
    public readonly CChromaColor Color;
    public readonly EffectDuration Duration;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct SpectrumCycling
{
    public readonly uint dwParam;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Starlight
{
    public readonly uint dwParam;
    public readonly BreathingEffectType Type;
    public readonly EffectDuration Duration;
    public readonly CChromaColor Color1;
    public readonly CChromaColor Color2;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Wave
{
    public readonly uint dwParam;
    public readonly EffectDirection Direction;
    public readonly int Speed;
}

[UnmanagedArray(typeof(char), 260)]
public readonly partial struct Wchar260
{
    public override string ToString()
    {
        var sb = new StringBuilder();

        for (var i = 0; i < Count; i++)
        {
            if (this[i] == 0)
                break;

            sb.Append(this[i]);
        }

        return sb.ToString();
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaDevice
{
    public readonly Wchar260 Instance;
    public readonly Wchar260 InstanceName;
}

[UnmanagedArray(typeof(CChromaDevice), 10)]
public readonly partial struct CChromaDevice10
{
    
}

public enum BreathingEffectType
{
    OneColor = 1,
    TwoColors = 2,
    RandomColors = 3,
}

public enum EffectType
{
    None = 0,
    Wave = 1,
    SpectrumCycling = 2,
    Breathing = 3,
    Blinking = 4,
    Reactive = 5,
    Static = 6,
    Custom = 7,
    CustomKey = 8,
    Init = 9,
    Uninit = 10,
    Default = 11,
    Starlight = 12,
    Suspend = 13,
    Resume = 14,
    Invalid = 15,
    Active = 16,
    Visualizer = 17
}

//wtf is this
public enum MouseLedType
{
    None = 0,
    Scrollwheel = 1,
    Logo = 2,
    Backlight = 4,
    SideStrip1 = 8,
    SideStrip2 = 16,
    SideStrip3 = 32,
    SideStrip4 = 64,
    SideStrip5 = 128,
    SideStrip6 = 256,
    SideStrip7 = 512,
    SideStrip8 = 1024,
    SideStrip9 = 2048,
    SideStrip10 = 65536,
    SideStrip11 = 131072,
    SideStrip12 = 262144,
    SideStrip13 = 524288,
    SideStrip14 = 1048576,
    SideStrip15 = 2097152,
    All = 4132863
};

public enum EffectDuration
{
    Short = 0,
    Medium = 1,
    Long = 2
}

public enum EffectDirection
{
    LeftToRight = 1,
    RightToLeft = 2,
    FrontToBack = 3,
    BackToFront = 4
};