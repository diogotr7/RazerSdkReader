using RazerSdkReader.Structures;
using System;

namespace RazerSdkReader.ConsoleApp;

public static class Program
{
    public static void Main()
    {
        var reader = new ChromaReader();
        reader.Exception += WriteException;
        reader.KeyboardUpdated += Write;
        reader.MouseUpdated += Write;
        reader.MousepadUpdated += Write;
        reader.KeypadUpdated += Write;
        reader.HeadsetUpdated += Write;
        reader.ChromaLinkUpdated += Write;
        reader.AppDataUpdated += WriteAppData;

        reader.Start();
        Console.ReadLine();
        reader.Dispose();
        Console.WriteLine("Disposed reader successfully...");
    }

    private static void Write<T>(object? _, in T e) where T : unmanaged, IColorProvider
    {
        var typeName = typeof(T).Name;
        var clr = e.GetColor(0);
        Console.WriteLine($"[{typeName}]Color: {clr}");
    }

    private static void WriteAppData(object? _, in ChromaAppData appData)
    {
        Console.WriteLine($"ChromaAppData active: {appData.CurrentAppName}");
    }

    private static void WriteException(object? _, Exception exception)
    {
        Console.WriteLine($"Exception: {exception}");
    }
}