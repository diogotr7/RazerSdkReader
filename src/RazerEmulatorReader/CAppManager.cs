﻿using System.Runtime.InteropServices;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly  struct CAppManagerData
{
    public readonly uint ProcessId;
    public readonly int Register;
    public readonly ulong Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CAppManager
{
    public readonly uint WriteIndex;
    public readonly uint Padding;
    public readonly CAppManager10 Data;
}

[UnmanagedArray(typeof(CAppManagerData), 10)]
public readonly partial struct CAppManager10
{
    
}
