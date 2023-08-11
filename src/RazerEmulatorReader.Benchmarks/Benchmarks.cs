using BenchmarkDotNet.Attributes;
using RazerEmulatorReader.Structures;

namespace RazerEmulatorReader.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
public class Benchmarks
{
    private readonly MemoryMappedStructReader<CChromaKeyboard> _reader = new(Constants.CChromaKeyboardFileMapping);
    private readonly MemoryMappedFileProxy1 _mmf1 = new(Constants.CChromaKeyboardFileMapping, (int)Constants.CChromaKeyboardSize);
    private readonly MemoryMappedFileProxy2 _mmf2 = new(Constants.CChromaKeyboardFileMapping, (int)Constants.CChromaKeyboardSize);
    private readonly MemoryMappedFileProxy3 _mmf3 = new(Constants.CChromaKeyboardFileMapping, (int)Constants.CChromaKeyboardSize);
    private readonly MemoryMappedFileProxy4 _mmf4 = new(Constants.CChromaKeyboardFileMapping, (int)Constants.CChromaKeyboardSize);

    [Benchmark]
    public KeyboardData MemoryStream_Aurora()
    {
        return _mmf1.Read<KeyboardData>();
    }

    [Benchmark]
    public KeyboardData PositionOffset()
    {
        return _mmf2.Read<KeyboardData>();
    }

    [Benchmark]
    public KeyboardData PtrToStructureUnsafe()
    {
        return _mmf3.Read<KeyboardData>();
    }

    [Benchmark]
    public CChromaKeyboard AsRefUnsafe()
    {
        return _mmf4.Read<CChromaKeyboard>();
    }

    [Benchmark]
    public CChromaKeyboard MemoryMappedStructReader()
    {
        return _reader.Read();
    }
}