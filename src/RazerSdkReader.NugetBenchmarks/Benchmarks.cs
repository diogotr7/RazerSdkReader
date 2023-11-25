using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using RazerSdkReader.Structures;

[Config(typeof(MyConfig))]
public class Benchmarks
{
    private class MyConfig : ManualConfig
    {
        public MyConfig()
        {
            var baseJob = Job.ShortRun;

            AddJob(baseJob.WithNuGet("RazerSdkReader", "1.1.0").WithBaseline(true));
            AddJob(baseJob.WithNuGet("RazerSdkReader", "1.6.0"));
            AddJob(baseJob.WithNuGet("RazerSdkReader", "1.7.0"));
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
}