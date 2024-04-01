using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using RazerSdkReader.Structures;

namespace RazerSdkReader.Benchmarks;

[ShortRunJob]
public class ColorReadingBenchmarks
{
    private readonly ChromaKeyboard _keyboard;
    private readonly ChromaColor[] _colors;
    
    public ColorReadingBenchmarks()
    {
        _colors = new ChromaColor[ChromaKeyboard.COUNT];
        var bytes = File.ReadAllBytes(Path.Combine(GetCallerFilePath(), "..", "keyboard.bin"));
        _keyboard = MemoryMarshal.Read<ChromaKeyboard>(bytes);
    }

    private static string GetCallerFilePath([CallerFilePath] string callerFilePath = "") => callerFilePath;
    
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