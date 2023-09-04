using System.Text;
using UnmanagedArrayGenerator;

namespace RazerSdkReader.Structures;

[UnmanagedArray(typeof(char), ChromaStringLength)]
public readonly partial record struct ChromaString
{
    private const int ChromaStringLength = 260;
    
    public override string ToString()
    {
        var sb = new StringBuilder();

        for (var i = 0; i < ChromaStringLength; i++)
        {
            var c = this[i];
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