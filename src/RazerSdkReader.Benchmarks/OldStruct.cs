using System.Runtime.InteropServices;

namespace RazerSdkReader.Benchmarks;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct KeyboardData
{
    public readonly int SnapshotCounter;

    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 10)]
    public readonly SnapshotData[] Snapshots;
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct SnapshotData
{
    public readonly int Unknown0;

    public readonly ushort Unknown1;
    public readonly ushort DeviceType;

    public readonly int EffectType;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public readonly int[] Unknown2;

    public readonly ZoneData StaticZone;

    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 132)]
    public readonly ZoneData[] CustomGrid;

    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 132)]
    public readonly ZoneData[] CustomGridColor0;

    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 132)]
    public readonly ZoneData[] CustomGridKey0;

    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 23)]
    public readonly int[] Unknown3;

    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 144)]
    public readonly ZoneData[] ExtendedGridColor1;

    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 144)]
    public readonly ZoneData[] ExtendedGridKey1;

    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 14)]
    public readonly int[] Unknown4;

    public readonly uint Timestamp;
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct ZoneData
{
    public readonly byte R;
    public readonly byte G;
    public readonly byte B;
    public readonly byte A;
};