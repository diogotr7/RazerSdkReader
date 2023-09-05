using System.Text;
using UnmanagedArrayGenerator;

namespace RazerSdkReader.Structures;

[UnmanagedArray(typeof(char), 260)]
public readonly partial record struct ChromaString
{
    public override string ToString()
    {
        var sb = new StringBuilder();

        foreach (ref readonly var c in AsSpan())
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