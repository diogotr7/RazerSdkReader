using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using RazerEmulatorReader.Structures;

namespace RazerEmulatorReader.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
public class Benchmarks
{
    private readonly MemoryMappedFile _file;
    private readonly MemoryMappedViewAccessor _view;
    private readonly MemoryMappedFileProxy1 _mmf1;
    private readonly MemoryMappedFileProxy2 _mmf2;
    private readonly MemoryMappedFileProxy3 _mmf3;
    private readonly MemoryMappedFileProxy4 _mmf4;

    public Benchmarks()
    {
        _file = MemoryMappedFile.OpenExisting(Constants.CChromaKeyboardFileMapping, MemoryMappedFileRights.Read);
        _view = _file.CreateViewAccessor(0, (int)Constants.CChromaKeyboardSize, MemoryMappedFileAccess.Read);
        _mmf1 = new MemoryMappedFileProxy1(Constants.CChromaKeyboardFileMapping, (int)Constants.CChromaKeyboardSize);
        _mmf2 = new MemoryMappedFileProxy2(Constants.CChromaKeyboardFileMapping, (int)Constants.CChromaKeyboardSize);
        _mmf3 = new MemoryMappedFileProxy3(Constants.CChromaKeyboardFileMapping, (int)Constants.CChromaKeyboardSize);
        _mmf4 = new MemoryMappedFileProxy4(Constants.CChromaKeyboardFileMapping, (int)Constants.CChromaKeyboardSize);
    }

    [Benchmark]
    public void ReadKeyboardDataDirect()
    {
        _view.Read<CChromaKeyboard>(0, out var T);
    }
    
    [Benchmark]
    public void ReadKeyboardDataSafeHandle()
    {
        _view.SafeMemoryMappedViewHandle.Read<CChromaKeyboard>(0);
    }
    
    [Benchmark]
    public void ReadKeyboardData1()
    {
        _mmf1.Read<KeyboardData>();
    }

    [Benchmark]
    public void ReadKeyboardData2()
    {
        _mmf2.Read<KeyboardData>();
    }
    
    [Benchmark]
    public void ReadKeyboardData3()
    {
        _mmf3.Read<KeyboardData>();
    }
    
    
    [Benchmark]
    public void ReadKeyboardData4()
    {
        _mmf4.Read<CChromaKeyboard>();
    }
    
    [Benchmark]
    public unsafe void ReadKeyboardData5()
    {
        var handle = _view.SafeMemoryMappedViewHandle.DangerousGetHandle();
        ref var x =  ref Unsafe.As<byte, CChromaKeyboard>(ref *(byte*)handle);
        var effect = x.Data[0].Timestamp;
    }
}
