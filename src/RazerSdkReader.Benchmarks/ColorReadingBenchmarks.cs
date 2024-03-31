using BenchmarkDotNet.Attributes;
using RazerSdkReader.Structures;

namespace RazerSdkReader.Benchmarks;

[ShortRunJob]
public class ColorReadingBenchmarks
{
    private readonly ChromaKeyboard _keyboard = new();
    private readonly ChromaColor[] _colors = new ChromaColor[ChromaKeyboard.COUNT];
    
    [Benchmark]
    public void GetColorsOneByOne()
    {
        for (var i = 0; i < ChromaKeyboard.COUNT; i++)
        {
            _colors[i] = _keyboard.GetColor(i);
        }
    }

    [Benchmark]
    public void GetColors()
    {
        _keyboard.GetColors(_colors);
    }
}