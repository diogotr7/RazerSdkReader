using BenchmarkDotNet.Attributes;
using RazerSdkReader.Extensions;
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

    [Benchmark(Baseline =true)]
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
    public void Decrypt()
    {
        ref readonly var lastSnapshot = ref _keyboard.Data[_keyboard.WriteIndex.ToReadIndex()];
        ReadOnlySpan<ChromaColor> data = lastSnapshot.Effect.Custom.Color;
        Span<ChromaColor> output = stackalloc ChromaColor[data.Length];
        ChromaEncryption.Decrypt(data, output, lastSnapshot.Timestamp);
    }
}