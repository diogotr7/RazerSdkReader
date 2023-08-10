using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace RazerEmulatorReader.Benchmarks;

public sealed class MemoryMappedFileProxyOld : IDisposable
{
    private MemoryMappedFile _file;
    private MemoryMappedViewStream _view;
    private readonly byte[] _array;
    private int _offset;
    private int _read;

    public string Name { get; }
    public int Size { get; }

    public MemoryMappedFileProxyOld(string name, int size)
    {
        Name = name;
        Size = size;
        _array = new byte[Size];

        try
        {
            _file = MemoryMappedFile.OpenExisting(Name, MemoryMappedFileRights.Read);
        }
        catch (FileNotFoundException)
        {
            _file = MemoryMappedFile.CreateNew(Name, Size, MemoryMappedFileAccess.Read, MemoryMappedFileOptions.None, HandleInheritability.None);
        }

        _view = _file.CreateViewStream(0, Size, MemoryMappedFileAccess.Read);
    }

    public T Read<T>() where T : struct
    {
        T result;
        var memory = Read();
        var handle = GCHandle.Alloc(memory, GCHandleType.Pinned);

        try
        {
            result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
        }
        finally
        {
            handle.Free();
        }

        return result;
    }

    public byte[] Read()
    {
        if (_view.Position != 0)
            _view.Seek(0, SeekOrigin.Begin);

        _offset = 0;

        while ((_read = _view.Read(_array, _offset, Size - _offset)) > 0)
            _offset += _read;

        return _array;
    }

    private void Dispose(bool disposing)
    {
        _view.Dispose();
        _file.Dispose();

        _view = null;
        _file = null;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public override string ToString() => $"[{Name}, {Size}]";
}


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