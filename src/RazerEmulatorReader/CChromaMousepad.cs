using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaMousepad
{ 
    public readonly uint WriteIndex;
    public readonly uint Padding;
    public readonly CChromaMousepadData10 Data;
    public readonly CChromaDevice10 Device;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaMousepadData
{
    public readonly uint Flag;
    public readonly int EffectType;
    public readonly MousepadEffect Effect;
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct MousepadEffect
{
    public readonly Breathing Breathing;
    public readonly Wave Wave;
    public readonly Static Static;
    public readonly MousepadCustom Custom;
    public readonly MousepadCustom2 Custom2;
}

[UnmanagedArray(typeof(CChromaColor), 15)]
public readonly partial record struct MousepadCustom
{

}

[UnmanagedArray(typeof(CChromaColor), 20)]
public readonly partial record struct MousepadCustom2
{
    
}

[UnmanagedArray(typeof(CChromaMousepadData), 10)]
public readonly partial record struct CChromaMousepadData10
{

}