using BenchmarkDotNet.Attributes;
using RazerSdkReader.Structures;
using System.Runtime.InteropServices;

namespace RazerSdkReader.Benchmarks;

[ShortRunJob]
public class ColorReadingBenchmarks
{
    private readonly ChromaKeyboard _keyboard = new();
    
    [Params(5, ChromaKeyboard.COUNT)]
    public int Count { get; set; }

    [Benchmark(Baseline = true)]
    public void GetAllColorsOnce()
    {
        Span<ChromaColor> colors = stackalloc ChromaColor[Count];
        for (var i = 0; i < colors.Length; i++)
        {
            colors[i] = _keyboard.GetColor(i);
        }
    }
    
    [Benchmark]
    public void GetColorsSpan()
    {
        Span<ChromaColor> output = stackalloc ChromaColor[Count];
        _keyboard.GetColors(output);
    }
}