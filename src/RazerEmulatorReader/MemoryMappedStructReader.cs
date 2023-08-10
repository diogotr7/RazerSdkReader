using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;

namespace RazerEmulatorReader;

public sealed class MemoryMappedStructReader<T> : IDisposable where T : unmanaged
{
    private readonly MemoryMappedFile _file;
    private readonly MemoryMappedViewAccessor _view;

    public string Name { get; }
    public int Size { get; }

    public MemoryMappedStructReader(string name)
    {
        Name = name;
        Size = Unsafe.SizeOf<T>();

        _file = MemoryMappedFile.OpenExisting(Name, MemoryMappedFileRights.Read);
        _view = _file.CreateViewAccessor(0, Size, MemoryMappedFileAccess.Read);
    }

    public T Read()
    {
        return _view.SafeMemoryMappedViewHandle.Read<T>(0);
    }
    
    public void Dump(string path)
    {
        var array = new byte[Size];
        _view.ReadArray(0, array, 0, array.Length);
        File.WriteAllBytes(path, array);
    }

    public override string ToString() => $"[{Name}, {Size}]";

    public void Dispose()
    {
        _file.Dispose();
    }
}