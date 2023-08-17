using System.Diagnostics.Contracts;
using RazerSdkReader.Structures;

namespace RazerSdkReader;

public interface IColorProvider
{
    int Width { get; }
    int Height { get; }
    int Count { get; }

    [Pure]
    ChromaColor GetColor(int index);
}