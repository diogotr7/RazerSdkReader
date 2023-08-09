using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RazerEmulatorReader;

public sealed class MemoryMappedStructReader<T> : IDisposable where T : unmanaged
{
    private readonly MemoryMappedFile _file;
    private readonly MemoryMappedViewAccessor _view;

    public string Name { get; }
    public uint Size { get; }

    public MemoryMappedStructReader(string name, uint size)
    {
        if (Unsafe.SizeOf<T>() != size)
            throw new ArgumentException("Mismatched size and struct", nameof(size));
            
        Name = name;
        Size = size;

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