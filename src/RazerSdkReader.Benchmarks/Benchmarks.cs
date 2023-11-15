using BenchmarkDotNet.Attributes;
using RazerSdkReader.Structures;

namespace RazerSdkReader.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
public class Benchmarks
{
    private readonly MemoryMappedStructReader<ChromaKeyboard> _reader = new(Constants.KeyboardFileName);
    private readonly MemoryMappedFileProxy1 _mmf1 = new(Constants.KeyboardFileName, (int)Constants.KeyboardSize);
    private readonly MemoryMappedFileProxy2 _mmf2 = new(Constants.KeyboardFileName, (int)Constants.KeyboardSize);
    private readonly MemoryMappedFileProxy3 _mmf3 = new(Constants.KeyboardFileName, (int)Constants.KeyboardSize);
    private readonly MemoryMappedFileProxy4 _mmf4 = new(Constants.KeyboardFileName, (int)Constants.KeyboardSize);
    private readonly MemoryMappedFileProxy5 _mmf5 = new(Constants.KeyboardFileName, (int)Constants.KeyboardSize);

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
    public ChromaKeyboard AsRefUnsafe()
    {
        return _mmf4.Read<ChromaKeyboard>();
    }

    [Benchmark]
    public ChromaKeyboard MemoryMappedStructReader()
    {
        return _reader.Read();
    }

    [Benchmark]
    public ChromaKeyboard MemoryMappedFileProxy5()
    {
        return _mmf5.Read<ChromaKeyboard>();
    }
}