using System.Runtime.CompilerServices;
using System.Text;

namespace RazerSdkReader.Structures;

[InlineArray(260)]
public struct ChromaString
{
    public char _field;

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