using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace RazerSdkReader.Benchmarks;

public sealed class MemoryMappedFileProxy2 : IDisposable
{
    private MemoryMappedFile _file;
    private MemoryMappedViewStream _view;
    private readonly byte[] _array;
    private int _offset;
    private int _read;

    public string Name { get; }
    public int Size { get; }

    public MemoryMappedFileProxy2(string name, int size)
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