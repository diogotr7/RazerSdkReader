using System.Runtime.InteropServices;
using NUnit.Framework;

namespace RazerEmulatorReader.Tests;

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
        
        Assert.That(bytes, Has.Length.EqualTo(Constants.CAppManagerSize));
        
        Assert.DoesNotThrow(() =>
        {
            var appManager = MemoryMarshal.AsRef<CAppManager>(bytes);
        });
    }
    
    [Test]
    public void TestAppData()
    {
        var bytes = _data["app-data"];
        
        Assert.That(bytes, Has.Length.EqualTo(Constants.CAppDataSize));
        
        Assert.DoesNotThrow(() =>
        {
            var appData = MemoryMarshal.AsRef<CAppData>(bytes);
        });
    }
    
    [Test]
    public void TestHeadset()
    {
        var bytes = _data["headset"];
        
        Assert.That(bytes, Has.Length.EqualTo(Constants.CChromaHeadsetSize));
        
        Assert.DoesNotThrow(() =>
        {
            var headset = MemoryMarshal.AsRef<CChromaHeadset>(bytes);
        });
    }
    
    [Test]
    public void TestKeypad()
    {
        var bytes = _data["keypad"];
        
        Assert.That(bytes, Has.Length.EqualTo(Constants.CChromaKeypadSize));
        
        Assert.DoesNotThrow(() =>
        {
            var keypad = MemoryMarshal.AsRef<CChromaKeypad>(bytes);
        });
    }
    
    [Test]
    public void TestKeyboard()
    {
        var bytes = _data["keyboard"];
        
        Assert.That(bytes, Has.Length.EqualTo(Constants.CChromaKeyboardSize));
        
        Assert.DoesNotThrow(() =>
        {
            var keyboard = MemoryMarshal.AsRef<CChromaKeyboard>(bytes);
        });
    }
    
    [Test]
    public void TestLink()
    {
        var bytes = _data["link"];
        
        Assert.That(bytes, Has.Length.EqualTo(Constants.CChromaLinkSize));
        
        Assert.DoesNotThrow(() =>
        {
            var link = MemoryMarshal.AsRef<CChromaLink>(bytes);
        });
    }
    
    [Test]
    public void TestMouse()
    {
        var bytes = _data["mouse"];
        
        Assert.That(bytes, Has.Length.EqualTo(Constants.CChromaMouseSize));
        
        Assert.DoesNotThrow(() =>
        {
            var mouse = MemoryMarshal.AsRef<CChromaMouse>(bytes);
        });
    }
    
    [Test]
    public void TestMousepad()
    {
        var bytes = _data["mousepad"];
        
        Assert.That(bytes, Has.Length.EqualTo(Constants.CChromaMousepadSize));
        
        Assert.DoesNotThrow(() =>
        {
            var mousepad = MemoryMarshal.AsRef<CChromaMousepad>(bytes);
        });
    }
}