using RazerSdkReader.Structures;
using System.Diagnostics.Contracts;

namespace RazerSdkReader;

public interface IColorProvider
{
    int Width { get; }
    int Height { get; }
    int Count { get; }

    [Pure]
    ChromaColor GetColor(int index);
}