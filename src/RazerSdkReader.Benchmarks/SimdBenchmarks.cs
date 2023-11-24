using BenchmarkDotNet.Attributes;
using RazerSdkReader.Extensions;
using RazerSdkReader.Structures;

namespace RazerSdkReader.Benchmarks;

[ShortRunJob]
public class SimdBenchmarks
{
    [Params(2,5,15,50,200,1000)]
    public int Count { get; set; }

    [Benchmark]
    public void Regular()
    {
        Span<ChromaColor> colors = stackalloc ChromaColor[Count];
        Span<ChromaColor> output = stackalloc ChromaColor[Count];
        ChromaEncryption.Decrypt(colors, output, new ChromaTimestamp());
    }
    
    [Benchmark]
    public void Simd()
    {
        Span<ChromaColor> colors = stackalloc ChromaColor[Count];
        Span<ChromaColor> output = stackalloc ChromaColor[Count];
        ChromaEncryption.DecryptSimd(colors, output, new ChromaTimestamp());
    }
}