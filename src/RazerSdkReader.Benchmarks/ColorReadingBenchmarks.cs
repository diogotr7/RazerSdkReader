using BenchmarkDotNet.Attributes;
using RazerSdkReader.Structures;
using System.Runtime.InteropServices;

namespace RazerSdkReader.Benchmarks;

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
        ChromaColor x = default;
        var width = _keyboard.Width;
        var height = _keyboard.Height;
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                var index = i + (j * height);
                x ^= _keyboard.GetColor(index);
            }
        }
    }
}