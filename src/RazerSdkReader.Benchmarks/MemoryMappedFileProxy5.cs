using System.IO.MemoryMappedFiles;

namespace RazerSdkReader.Benchmarks;

public class MemoryMappedFileProxy5 : IDisposable
{
    private readonly MemoryMappedFile _file;
    private readonly MemoryMappedViewAccessor _view;

    internal string Name { get; }
    internal int Size { get; }

    internal MemoryMappedFileProxy5(string name, int size)
    {
        Name = name;
        Size = size;

        _file = MemoryMappedFile.OpenExisting(Name, MemoryMappedFileRights.Read);
        _view = _file.CreateViewAccessor(0, Size, MemoryMappedFileAccess.Read);
    }

    internal T Read<T>() where T : unmanaged
    {
        _view.Read<T>(0, out var x);

        return x;
    }

    protected virtual void Dispose(bool disposing)
    {
        _view.Dispose();
        _file.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public override string ToString() => $"[{Name}, {Size}]";
}