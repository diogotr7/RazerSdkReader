using System.Runtime.CompilerServices;
using System.Text;

namespace RazerSdkReader.Structures;

public readonly struct ChromaString
{
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
    private readonly Array260<char> _data;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (ref readonly var c in _data)
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

[InlineArray(5)]
public struct Array5<T> where T : unmanaged
{
    private T _field;
}

[InlineArray(10)]
public struct Array10<T> where T : unmanaged
{
    private T _field;
}

[InlineArray(15)]
public struct Array15<T> where T : unmanaged
{
    private T _field;
}

[InlineArray(20)]
public struct Array20<T> where T : unmanaged
{
    private T _field;
}

[InlineArray(30)]
public struct Array30<T> where T : unmanaged
{
    private T _field;
}

[InlineArray(50)]
public struct Array50<T> where T : unmanaged
{
    private T _field;
}

[InlineArray(63)]
public struct Array63<T> where T : unmanaged
{
    private T _field;
}

[InlineArray(132)]
public struct Array132<T> where T : unmanaged
{
    private T _field;
}

[InlineArray(192)]
public struct Array192<T> where T : unmanaged
{
    private T _field;
}

[InlineArray(260)]
public struct Array260<T> where T : unmanaged
{
    private T _field;
}