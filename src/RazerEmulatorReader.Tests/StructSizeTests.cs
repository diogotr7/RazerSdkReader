using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace RazerEmulatorReader.Tests;

public class StructSizeTests
{
    [Test]
    public void TestKeyboardStructSize()
    {
        Assert.That(Unsafe.SizeOf<CChromaKeyboard>(), Is.EqualTo(Constants.CChromaKeyboardSize));
    }

    [Test]
    public void TestMouseStructSize()
    {
        Assert.That(Unsafe.SizeOf<CChromaMouse>(), Is.EqualTo(Constants.CChromaMouseSize));
    }

    [Test]
    public void TestHeadsetStructSize()
    {
        Assert.That(Unsafe.SizeOf<CChromaHeadset>(), Is.EqualTo(Constants.CChromaHeadsetSize));
    }

    [Test]
    public void TestMousepadStructSize()
    {
        Assert.That(Unsafe.SizeOf<CChromaMousepad>(), Is.EqualTo(Constants.CChromaMousepadSize));
    }

    [Test]
    public void TestKeypadStructSize()
    {
        Assert.That(Unsafe.SizeOf<CChromaKeypad>(), Is.EqualTo(Constants.CChromaKeypadSize));
    }

    [Test]
    public void TestLinkStructSize()
    {
        Assert.That(Unsafe.SizeOf<CChromaLink>(), Is.EqualTo(Constants.CChromaLinkSize));
    }

    [Test]
    public void TestAppManagerStructSize()
    {
        Assert.That(Unsafe.SizeOf<CAppManager>(), Is.EqualTo(Constants.CAppManagerSize));
    }

    [Test]
    public void TestAppDataStructSize()
    {
        Assert.That(Unsafe.SizeOf<CAppData>(), Is.EqualTo(Constants.CAppDataSize));
    }
}