using BenchmarkDotNet.Attributes;
using RazerSdkReader.Structures;
using System.Runtime.InteropServices;

namespace RazerSdkReader.Benchmarks;

[ShortRunJob]
public class ColorReadingBenchmarks
{
    private readonly ChromaKeyboard _keyboard;

    public ColorReadingBenchmarks()
    {
        var bytes = File.ReadAllBytes("keyboard.bin");
        _keyboard = MemoryMarshal.Read<ChromaKeyboard>(bytes);
    }

    [Benchmark]
    public void GetAllColorsOnce()
    {
        var width = _keyboard.Width;
        var height = _keyboard.Height;
        Span<ChromaColor> colors = stackalloc ChromaColor[width * height];
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                var index = i + (j * height);
                colors[index] = _keyboard.GetColor(index);
            }
        }
    }
    
    [Benchmark]
    public void GetColorsSpan()
    {
        Span<ChromaColor> output = stackalloc ChromaColor[_keyboard.Count];
        _keyboard.GetColors(output);
    }
}