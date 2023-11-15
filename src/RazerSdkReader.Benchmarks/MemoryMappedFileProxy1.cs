using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace RazerSdkReader.Benchmarks;

public class MemoryMappedFileProxy1 : IDisposable
{
    private MemoryMappedFile _file;
    private MemoryMappedViewStream _view;

    public string Name { get; }
    public int Size { get; }

    public MemoryMappedFileProxy1(string name, int size, bool createNew = true)
    {
        Name = name;
        Size = size;

        _file = MemoryMappedFile.OpenExisting(Name, MemoryMappedFileRights.Read);
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

        using var memory = new MemoryStream(Size);
        const int bufferSize = 4096;
        var buffer = new byte[bufferSize];

        int read;
        while ((read = _view.Read(buffer, 0, bufferSize)) > 0)
            memory.Write(buffer, 0, read);

        return memory.ToArray();
    }

    protected virtual void Dispose(bool disposing)
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