using BenchmarkDotNet.Attributes;
using RazerEmulatorReader.Structures;

namespace RazerEmulatorReader.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class Benchmarks
{
    private readonly MemoryMappedStructReader<CChromaKeyboard> _mmf = new(Constants.CChromaKeyboardFileMapping);
    private readonly MemoryMappedFileProxyOld _mmf2 = new(Constants.CChromaKeyboardFileMapping, (int)Constants.CChromaKeyboardSize);

    [Benchmark]
    public void ReadKeyboardDataNew()
    {
        _mmf.Read();
    }

    [Benchmark]
    public void ReadKeyboardDataOld()
    {
        _mmf2.Read<KeyboardData>();
    }
}
