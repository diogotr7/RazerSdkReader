using RazerSdkReader.Structures;

namespace RazerSdkReader;

public interface IColorProvider
{
    int Width { get; }
    int Height { get; }
    int Count { get; }
    ChromaColor GetColor(int index);
}