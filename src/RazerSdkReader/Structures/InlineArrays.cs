using System.Runtime.CompilerServices;
using System.Text;

namespace RazerSdkReader.Structures;

#pragma warning disable RCS1169 // Make field read-only.
#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable RCS1213 // Remove unused member declaration.

[InlineArray(260)]
public struct ChromaString
{
    private char _field;

    public readonly override string ToString()
    {
        var sb = new StringBuilder();
        foreach (ref readonly var c in this)
        {
            if (c == 0)
                break;

            sb.Append(c);
        }

        return sb.ToString();
    }

    public static implicit operator string(ChromaString str)
    {
        return str.ToString();
    }
}

[InlineArray(50)]
public struct ChromaAppInfo50
{
    private ChromaAppInfo _field;
}

[InlineArray(10)]
public struct ChromaAppManager10
{
    private ChromaAppManagerData _field;
}

[InlineArray(50)]
public struct ChromaDeviceDataInfo50
{
    private ChromaDeviceDataInfo _field;
}

[InlineArray(5)]
public struct HeadsetCustom
{
    private ChromaColor _field;
}

[InlineArray(10)]
public struct ChromaHeadsetData10
{
    private ChromaHeadsetData _field;
}

[InlineArray(192)]
public struct Color8X24
{
    private ChromaColor _field;
}

[InlineArray(132)]
public struct Color6X22
{
    private ChromaColor _field;
}

[InlineArray(10)]
public struct ChromaKeyboardData10
{
    private ChromaKeyboardData _field;
}

[InlineArray(20)]
public struct KeypadCustom
{
    private ChromaColor _field;
}

[InlineArray(10)]
public struct ChromaKeypadData10
{
    private ChromaKeypadData _field;
}

[InlineArray(50)]
public struct LinkCustom
{
    private ChromaColor _field;
}

[InlineArray(10)]
public struct ChromaLinkData10
{
    private ChromaLinkData _field;
}

[InlineArray(30)]
public struct MouseCustom
{
    private ChromaColor _field;
}

[InlineArray(63)]
public struct MouseCustom2
{
    private ChromaColor _field;
}

[InlineArray(10)]
public struct ChromaMouseData10
{
    private ChromaMouseData _field;
}

[InlineArray(15)]
public struct MousepadCustom
{
    private ChromaColor _field;
}

[InlineArray(20)]
public struct MousepadCustom2
{
    private ChromaColor _field;
}

[InlineArray(10)]
public struct ChromaMousepadData10
{
    private ChromaMousepadData _field;
}

[InlineArray(10)]
public struct ChromaDevice10
{
    private ChromaDevice _field;
}
#pragma warning restore RCS1169 // Make field read-only.
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning restore RCS1213 // Remove unused member declaration.