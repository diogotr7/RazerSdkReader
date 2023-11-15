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
        for (var i = 0; i < _keyboard.Width; i++)
        {
            for (var j = 0; j < _keyboard.Height; j++)
            {
                var index = i + j * _keyboard.Width;
                x ^= _keyboard.GetColor(index);
            }
        }
    }
}