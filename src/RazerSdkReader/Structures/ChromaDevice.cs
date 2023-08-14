using System;
using System.Runtime.InteropServices;

namespace RazerSdkReader.Structures;

/// <summary>
///     Unused. Kept for completeness.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaDeviceData
{
    public readonly int Count;
    public readonly ChromaDeviceDataInfo50 Device;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct ChromaDeviceDataInfo
{
    public readonly ChromaString Instance;
    public readonly ChromaString EventName;
    public readonly ChromaString FileName;
    public readonly ChromaString ModuleName;
    public readonly ChromaString DevicePath;
    public readonly Guid DeviceId;
}

[UnmanagedArray(typeof(ChromaDeviceDataInfo), 50)]
public readonly partial record struct ChromaDeviceDataInfo50;