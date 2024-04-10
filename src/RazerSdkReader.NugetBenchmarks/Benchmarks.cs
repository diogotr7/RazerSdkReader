using System.Runtime.CompilerServices;
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

            AddJob(baseJob.WithNuGet("RazerSdkReader", "1.07.0").WithBaseline(true));
            AddJob(baseJob.WithNuGet("RazerSdkReader", "2.0.0"));
            AddJob(baseJob.WithNuGet("RazerSdkReader", "2.1.0"));
        }
    }
    
    private readonly ChromaKeyboard _keyboard;
    private readonly ChromaColor[] _colors;

    private static string GetCallerFilePath([CallerFilePath] string callerFilePath = "") => callerFilePath;

    public Benchmarks()
    {
        _colors = new ChromaColor[ChromaKeyboard.COUNT];
        var bytes = File.ReadAllBytes(Path.Combine(GetCallerFilePath(), "..", "keyboard.bin"));
        _keyboard = MemoryMarshal.Read<ChromaKeyboard>(bytes);
    }

    [Benchmark]
    public void GetColors()
    {
        _keyboard.GetColors(_colors);
    }
}