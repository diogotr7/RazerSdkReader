using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using RazerSdkReader.Structures;

namespace RazerSdkReader.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
public unsafe class MemoryReadingBenchmarks
{
    private readonly MemoryMappedFile _file;
    private readonly MemoryMappedViewAccessor _viewAccessor;
    private readonly MemoryMappedViewStream _viewStream;
    private const int Size = (int)Constants.KeyboardSize;
    private readonly IntPtr _pointer;

    public MemoryReadingBenchmarks()
    {
        var size = Unsafe.SizeOf<ChromaKeyboard>();
        _file = MemoryMappedFile.OpenExisting(Constants.KeyboardFileName, MemoryMappedFileRights.Read);
        _viewAccessor = _file.CreateViewAccessor(0, size, MemoryMappedFileAccess.Read);
        _viewStream = _file.CreateViewStream(0, size, MemoryMappedFileAccess.Read);
        
        byte* ptr = null;
        _viewAccessor.SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);
        _pointer = new IntPtr(ptr);
    }
    
    
    private readonly MemoryMappedStructReader<ChromaKeyboard> _reader = new(Constants.KeyboardFileName);
    
    [Benchmark]
    public KeyboardData MemoryStreamPtrToStructure()
    {
        if (_viewStream.Position != 0)
            _viewStream.Seek(0, SeekOrigin.Begin);

        using var memorySteam = new MemoryStream(Size);
        const int bufferSize = 4096;
        var buffer = new byte[bufferSize];

        int read;
        
        while ((read = _viewStream.Read(buffer, 0, bufferSize)) > 0)
            memorySteam.Write(buffer, 0, read);

        KeyboardData result;
        var memory = memorySteam.ToArray();
        var handle = GCHandle.Alloc(memory, GCHandleType.Pinned);

        try
        {
            result = (KeyboardData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(KeyboardData));
        }
        finally
        {
            handle.Free();
        }

        return result;
    }

    private readonly byte[] _array = new byte[Size];
    private int _offset;
    private int _read;
    

    [Benchmark]
    public KeyboardData PositionOffsetPtrToStructure()
    {
        if (_viewStream.Position != 0)
            _viewStream.Seek(0, SeekOrigin.Begin);

        _offset = 0;

        while ((_read = _viewStream.Read(_array, _offset, Size - _offset)) > 0)
            _offset += _read;

        KeyboardData result;
        var handle = GCHandle.Alloc(_array, GCHandleType.Pinned);

        try
        {
            result = (KeyboardData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(KeyboardData));
        }
        finally
        {
            handle.Free();
        }

        return result;
    }

    [Benchmark]
    public KeyboardData PtrToStructureUnsafe()
    {
        return Marshal.PtrToStructure<KeyboardData>(_pointer);
    }

    [Benchmark]
    public ChromaKeyboard AsRefUnsafe()
    {
        return MemoryMarshal.AsRef<ChromaKeyboard>(new ReadOnlySpan<byte>(_pointer.ToPointer(), Size));
    }

    [Benchmark]
    public ChromaKeyboard MemoryMappedStructReader()
    {
        return Unsafe.AsRef<ChromaKeyboard>(_viewAccessor.SafeMemoryMappedViewHandle.DangerousGetHandle().ToPointer());
    }

    [Benchmark]
    public ChromaKeyboard MemoryMappedFileProxy5()
    {
        _viewAccessor.Read<ChromaKeyboard>(0, out var x);

        return x;
    }
}