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

            AddJob(baseJob.WithNuGet("RazerSdkReader", "1.07.0").WithBaseline(true));
            AddJob(baseJob.WithNuGet("RazerSdkReader", "1.08.0"));
            AddJob(baseJob.WithNuGet("RazerSdkReader", "1.09.0")); 
            AddJob(baseJob.WithNuGet("RazerSdkReader", "1.10.0"));
            AddJob(baseJob.WithNuGet("RazerSdkReader", "1.11.0"));
            AddJob(baseJob.WithNuGet("RazerSdkReader", "2.0.0"));
        }
    }
    
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