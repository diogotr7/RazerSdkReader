using System.Runtime.InteropServices;
using NUnit.Framework;
using RazerSdkReader.Structures;

namespace RazerSdkReader.Tests;

public class ReadFromFileTests
{
    private Dictionary<string, byte[]> _data;

    [SetUp]
    public void Setup()
    {
        //grab all *.bin from TestData
        //read each file into a byte[]
        //Cast from byte[] to struct

        var files = Directory.GetFiles("TestData", "*.bin");

        _data = new();

        foreach (var file in files)
        {
            var bytes = File.ReadAllBytes(file);
            _data.Add(Path.GetFileNameWithoutExtension(file), bytes);
        }
    }

    [Test]
    public void TestAppManager()
    {
        var bytes = _data["app-manager"];

        Assert.That(bytes, Has.Length.EqualTo(Constants.AppManagerSize));

        Assert.DoesNotThrow(() =>
        {
            var appManager = MemoryMarshal.AsRef<ChromaAppManager>(bytes);
        });
    }

    [Test]
    public void TestAppData()
    {
        var bytes = _data["app-data"];

        Assert.That(bytes, Has.Length.EqualTo(Constants.AppDataSize));

        Assert.DoesNotThrow(() =>
        {
            var appData = MemoryMarshal.AsRef<ChromaAppData>(bytes);
        });
    }

    [Test]
    public void TestHeadset()
    {
        var bytes = _data["headset"];

        Assert.That(bytes, Has.Length.EqualTo(Constants.HeadsetSize));

        Assert.DoesNotThrow(() =>
        {
            var headset = MemoryMarshal.AsRef<ChromaHeadset>(bytes);
        });
    }

    [Test]
    public void TestKeypad()
    {
        var bytes = _data["keypad"];

        Assert.That(bytes, Has.Length.EqualTo(Constants.KeypadSize));

        Assert.DoesNotThrow(() =>
        {
            var keypad = MemoryMarshal.AsRef<ChromaKeypad>(bytes);
        });
    }

    [Test]
    public void TestKeyboard()
    {
        var bytes = _data["keyboard"];

        Assert.That(bytes, Has.Length.EqualTo(Constants.KeyboardSize));

        Assert.DoesNotThrow(() =>
        {
            var keyboard = MemoryMarshal.AsRef<ChromaKeyboard>(bytes);
        });
    }

    [Test]
    public void TestLink()
    {
        var bytes = _data["link"];

        Assert.That(bytes, Has.Length.EqualTo(Constants.LinkSize));

        Assert.DoesNotThrow(() =>
        {
            var link = MemoryMarshal.AsRef<ChromaLink>(bytes);
        });
    }

    [Test]
    public void TestMouse()
    {
        var bytes = _data["mouse"];

        Assert.That(bytes, Has.Length.EqualTo(Constants.MouseSize));

        Assert.DoesNotThrow(() =>
        {
            var mouse = MemoryMarshal.AsRef<ChromaMouse>(bytes);
        });
    }

    [Test]
    public void TestMousepad()
    {
        var bytes = _data["mousepad"];

        Assert.That(bytes, Has.Length.EqualTo(Constants.MousepadSize));

        Assert.DoesNotThrow(() =>
        {
            var mousepad = MemoryMarshal.AsRef<ChromaMousepad>(bytes);
        });
    }
}