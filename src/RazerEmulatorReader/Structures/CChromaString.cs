using System.Text;

namespace RazerEmulatorReader.Structures;

[UnmanagedArray(typeof(char), 260)]
public readonly partial record struct CChromaString
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

    public static implicit operator string(CChromaString str)
    {
        return str.ToString();
    }
}