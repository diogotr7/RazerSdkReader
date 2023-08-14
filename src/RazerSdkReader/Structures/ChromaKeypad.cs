using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaKeypad
{ 
    public readonly int WriteIndex;
    public readonly uint Padding;
    public readonly ChromaKeypadData10 Data;
    public readonly ChromaDevice10 Device;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaKeypadData
{
    public readonly uint Flag;
    public readonly int EffectType;
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