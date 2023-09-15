using System;
using System.Runtime.CompilerServices;
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

[InlineArray(50)]
public struct ChromaDeviceDataInfo50
{
    public ChromaDeviceDataInfo _field;
}