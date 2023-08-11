using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace RazerEmulatorReader.Benchmarks;

public unsafe class MemoryMappedFileProxy4 : IDisposable
{
    private readonly MemoryMappedFile _file;
    private readonly MemoryMappedViewAccessor _view;
    private readonly IntPtr _pointer;

    internal string Name { get; }
    internal int Size { get; }

    internal MemoryMappedFileProxy4(string name, int size)
    {
        Name = name;
        Size = size;

        try
        {
            _file = MemoryMappedFile.OpenExisting(Name, MemoryMappedFileRights.Read);
        }
        catch (FileNotFoundException)
        {
            _file = MemoryMappedFile.CreateNew(Name, Size, MemoryMappedFileAccess.Read, MemoryMappedFileOptions.None, HandleInheritability.None);
        }
        _view = _file.CreateViewAccessor(0, Size, MemoryMappedFileAccess.Read);
        
        byte* ptr = null;
        _view.SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);
        _pointer = new IntPtr(ptr);
    }

    internal T Read<T>() where T : unmanaged
    {
        return MemoryMarshal.AsRef<T>(new ReadOnlySpan<byte>(_pointer.ToPointer(), Size));
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