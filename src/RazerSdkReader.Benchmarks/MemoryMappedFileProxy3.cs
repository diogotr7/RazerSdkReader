using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace RazerSdkReader.Benchmarks;

public unsafe class MemoryMappedFileProxy3 : IDisposable
{
    private readonly MemoryMappedFile _file;
    private readonly MemoryMappedViewAccessor _view;
    private readonly IntPtr _pointer;

    internal string Name { get; }
    internal int Size { get; }

    internal MemoryMappedFileProxy3(string name, int size)
    {
        Name = name;
        Size = size;

        _file = MemoryMappedFile.OpenExisting(Name, MemoryMappedFileRights.Read);
        _view = _file.CreateViewAccessor(0, Size, MemoryMappedFileAccess.Read);

        byte* ptr = null;
        _view.SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);
        _pointer = new IntPtr(ptr);
    }

    internal T Read<T>() where T : struct
    {
        return Marshal.PtrToStructure<T>(_pointer);
    }

    protected virtual void Dispose(bool disposing)
    {
        _view.SafeMemoryMappedViewHandle.ReleasePointer();
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