using System.Text;

namespace RazerSdkReader.Structures;

[UnmanagedArray(typeof(char), 260)]
public readonly partial record struct ChromaString
{
    public override string ToString()
    {
        var sb = new StringBuilder();

        for (var i = 0; i < Count; i++)
        {
            if (this[i] == 0)
                break;

            sb.Append(this[i]);
        }

        return sb.ToString();
    }

    public static implicit operator string(ChromaString str)
    {
        return str.ToString();
    }
}