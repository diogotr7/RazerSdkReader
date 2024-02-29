using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using RazerSdkReader.Structures;

namespace RazerSdkReader.NugetBenchmarks;

[Config(typeof(MyConfig))]
public class Benchmarks
{
    private class MyConfig : ManualConfig
    {
        public MyConfig()
        {
            var baseJob = Job.ShortRun;

            AddJob(baseJob.WithNuGet("RazerSdkReader", "1.6.0").WithBaseline(true));
            AddJob(baseJob.WithNuGet("RazerSdkReader", "1.7.0"));
            AddJob(baseJob.WithNuGet("RazerSdkReader", "1.8.0"));
        }
    }
    
    private readonly ChromaKeyboard _keyboard;

    public Benchmarks()
    {
        var bytes = File.ReadAllBytes("keyboard.bin");
        _keyboard = MemoryMarshal.Read<ChromaKeyboard>(bytes);
    }

    [Benchmark]
    public void GetAllColorsOnce()
    {
        Span<ChromaColor> colors = stackalloc ChromaColor[_keyboard.Count];
        for (var i = 0; i < colors.Length; i++)
        {
            colors[i] = _keyboard.GetColor(i);
        }
    }
}