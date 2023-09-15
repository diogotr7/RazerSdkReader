using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace RazerSdkReader.Structures;

[InlineArray( 260)]
public struct ChromaString
{
    public char _field;
    
    public override string ToString()
    {
        var sb = new StringBuilder();
        Span<char> x = this;
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