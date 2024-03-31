using BenchmarkDotNet.Attributes;
using RazerSdkReader.Structures;

namespace RazerSdkReader.Benchmarks;

[ShortRunJob]
public class ColorReadingBenchmarks
{
    private readonly ChromaKeyboard _keyboard = new();
    private readonly ChromaColor[] _colors = new ChromaColor[ChromaKeyboard.COUNT];
    
    [Params(ChromaKeyboard.COUNT)]
    public int Count { get; set; }

    [Benchmark(Baseline = true)]
    public void GetAllColorsOnce()
    {
        for (var i = 0; i < _colors.Length; i++)
        {
            _colors[i] = _keyboard.GetColor(i);
        }
    }
    
    [Benchmark]
    public void GetColorsSpan()
    {
        _keyboard.GetColors(_colors);
    }
}