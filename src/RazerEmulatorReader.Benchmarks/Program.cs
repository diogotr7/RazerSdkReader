using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using RazerEmulatorReader.Attributes;

namespace RazerEmulatorReader.Benchmarks;

public static class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<Benchmarks>();
        return;
        using var reader2 = new MemoryMappedStructReader<CChromaKeyboard>(Constants.CChromaKeyboardFileMapping, Constants.CChromaKeyboardSize);
        using var reader3 = new MemoryMappedStructReader<CChromaMouse>(Constants.CChromaMouseFileMapping, Constants.CChromaMouseSize);
        using var reader4 = new MemoryMappedStructReader<CChromaHeadset>(Constants.CChromaHeadsetFileMapping, Constants.CChromaHeadsetSize);
        using var reader5 = new MemoryMappedStructReader<CChromaMousepad>(Constants.CChromaMousepadFileMapping, Constants.CChromaMousepadSize);
        using var reader6 = new MemoryMappedStructReader<CChromaKeypad>(Constants.CChromaKeypadFileMapping, Constants.CChromaKeypadSize);
        using var reader7 = new MemoryMappedStructReader<CChromaLink>(Constants.CChromaLinkFileMapping, Constants.CChromaLinkSize);
        using var reader8 = new MemoryMappedStructReader<CAppManager>(Constants.CAppManagerFileMapping, Constants.CAppManagerSize);
        using var reader9 = new MemoryMappedStructReader<CAppData>(Constants.CAppDataFileMapping, Constants.CAppDataSize);
        
        reader9.Dump("CAppData.bin");
        reader8.Dump("CAppManager.bin");
    }
}