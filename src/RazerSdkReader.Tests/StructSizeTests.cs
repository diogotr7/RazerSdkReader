using NUnit.Framework;
using RazerSdkReader.Structures;
using System.Runtime.CompilerServices;

namespace RazerSdkReader.Tests;

public class StructSizeTests
{
    [Test]
    public void TestKeyboardStructSize()
    {
        Assert.That(Unsafe.SizeOf<ChromaKeyboard>(), Is.EqualTo(Constants.KeyboardSize));
    }

    [Test]
    public void TestMouseStructSize()
    {
        Assert.That(Unsafe.SizeOf<ChromaMouse>(), Is.EqualTo(Constants.MouseSize));
    }

    [Test]
    public void TestHeadsetStructSize()
    {
        Assert.That(Unsafe.SizeOf<ChromaHeadset>(), Is.EqualTo(Constants.HeadsetSize));
    }

    [Test]
    public void TestMousepadStructSize()
    {
        Assert.That(Unsafe.SizeOf<ChromaMousepad>(), Is.EqualTo(Constants.MousepadSize));
    }

    [Test]
    public void TestKeypadStructSize()
    {
        Assert.That(Unsafe.SizeOf<ChromaKeypad>(), Is.EqualTo(Constants.KeypadSize));
    }

    [Test]
    public void TestLinkStructSize()
    {
        Assert.That(Unsafe.SizeOf<ChromaLink>(), Is.EqualTo(Constants.LinkSize));
    }

    [Test]
    public void TestAppManagerStructSize()
    {
        Assert.That(Unsafe.SizeOf<ChromaAppManager>(), Is.EqualTo(Constants.AppManagerSize));
    }

    [Test]
    public void TestAppDataStructSize()
    {
        Assert.That(Unsafe.SizeOf<ChromaAppData>(), Is.EqualTo(Constants.AppDataSize));
    }
}