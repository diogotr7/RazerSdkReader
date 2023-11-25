using BenchmarkDotNet.Attributes;
using RazerSdkReader.Extensions;
using RazerSdkReader.Structures;

namespace RazerSdkReader.Benchmarks;

[ShortRunJob]
public class SimdBenchmarks
{
    [Params(ChromaHeadset.COUNT, ChromaKeyboard.COUNT, ChromaKeypad.COUNT, ChromaMouse.COUNT, 1000)]
    public int Count { get; set; }

    [Benchmark]
    public void Regular()
    {
        Span<ChromaColor> colors = stackalloc ChromaColor[Count];
        Span<ChromaColor> output = stackalloc ChromaColor[Count];
#pragma warning disable CS0618 // Type or member is obsolete
        ChromaEncryption.DecryptSingle(colors, output, new ChromaTimestamp());
#pragma warning restore CS0618 // Type or member is obsolete
    }

    [Benchmark]
    public void Simd()
    {
        Span<ChromaColor> colors = stackalloc ChromaColor[Count];
        Span<ChromaColor> output = stackalloc ChromaColor[Count];
        ChromaEncryption.Decrypt(colors, output, new ChromaTimestamp());
    }
}