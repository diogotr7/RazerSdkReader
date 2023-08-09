﻿using System;
using System.Runtime.InteropServices;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CChromaDeviceData
{
    [MarshalAs(UnmanagedType.I4)]
    public readonly int Count;
        
    public readonly CChromaDeviceDataInfo50 Device;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public readonly struct CChromaDeviceDataInfo
{
    public readonly Wchar260 Instance;

    public readonly Wchar260 EventName;

    public readonly Wchar260 FileName;

    public readonly Wchar260 ModuleName;

    public readonly Wchar260 DevicePath;

    public readonly Guid DeviceId;
}

[UnmanagedArray(typeof(CChromaDeviceDataInfo), 50)]
public readonly partial struct CChromaDeviceDataInfo50
{ }
