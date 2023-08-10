using System;
using System.Runtime.InteropServices;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaDeviceData
{
    public readonly int Count;
    public readonly CChromaDeviceDataInfo50 Device;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct CChromaDeviceDataInfo
{
    public readonly Wchar260 Instance;
    public readonly Wchar260 EventName;
    public readonly Wchar260 FileName;
    public readonly Wchar260 ModuleName;
    public readonly Wchar260 DevicePath;
    public readonly Guid DeviceId;
}

[UnmanagedArray(typeof(CChromaDeviceDataInfo), 50)]
public readonly partial record struct CChromaDeviceDataInfo50
{
    
}
