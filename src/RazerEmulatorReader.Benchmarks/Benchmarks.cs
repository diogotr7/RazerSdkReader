using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using RazerEmulatorReader.Structures;

namespace RazerEmulatorReader.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
public class Benchmarks
{
    private readonly MemoryMappedViewAccessor _view;
    private readonly MemoryMappedFileProxy1 _mmf1;
    private readonly MemoryMappedFileProxy2 _mmf2;
    private readonly MemoryMappedFileProxy3 _mmf3;
    private readonly MemoryMappedFileProxy4 _mmf4;

    public Benchmarks()
    {
        var file = MemoryMappedFile.OpenExisting(Constants.CChromaKeyboardFileMapping, MemoryMappedFileRights.Read);
        _view = file.CreateViewAccessor(0, (int)Constants.CChromaKeyboardSize, MemoryMappedFileAccess.Read);
        _mmf1 = new MemoryMappedFileProxy1(Constants.CChromaKeyboardFileMapping, (int)Constants.CChromaKeyboardSize);
        _mmf2 = new MemoryMappedFileProxy2(Constants.CChromaKeyboardFileMapping, (int)Constants.CChromaKeyboardSize);
        _mmf3 = new MemoryMappedFileProxy3(Constants.CChromaKeyboardFileMapping, (int)Constants.CChromaKeyboardSize);
        _mmf4 = new MemoryMappedFileProxy4(Constants.CChromaKeyboardFileMapping, (int)Constants.CChromaKeyboardSize);
    }

    //[Benchmark]
    public void MemoryStream()
    {
        _mmf1.Read<KeyboardData>();
    }

    //[Benchmark]
    public void PositionOffset()
    {
        _mmf2.Read<KeyboardData>();
    }

    //[Benchmark]
    public void PtrToStructureUnsafe()
    {
        _mmf3.Read<KeyboardData>();
    }

    [Benchmark]
    public void AsRefUnsafe()
    {
        var x = _mmf4.Read<CChromaKeyboard>();
        ReadSomeProperties(in x);
    }

    [Benchmark]
    public void ViewDirectRead()
    {
        _view.Read<CChromaKeyboard>(0, out var x);
        ReadSomeProperties(in x);
    }

    [Benchmark]
    public void ViewHandleRead()
    {
        var x = _view.SafeMemoryMappedViewHandle.Read<CChromaKeyboard>(0);
        ReadSomeProperties(in x);
    }

    [Benchmark]
    public unsafe void UnsafeAs()
    {
        var handle = _view.SafeMemoryMappedViewHandle.DangerousGetHandle();
        ref var x = ref Unsafe.As<byte, CChromaKeyboard>(ref *(byte*)handle);
        ReadSomeProperties(in x);
    }

    public void ReadSomeProperties(in CChromaKeyboard kb)
    {
        var custom2 = kb.Data[kb.WriteIndex].Effect.Custom2;
        var custom3 = kb.Data[kb.WriteIndex].Effect.Custom3;
    }
}