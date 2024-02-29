using RazerSdkReader.Structures;
using System;

namespace RazerSdkReader;

/// <summary>
///     Represents a Razer Chroma device.
/// </summary>
public interface IColorProvider
{
    /// <summary>
    ///     Gets the width of the grid.
    /// </summary>
    int Width { get; }
    
    /// <summary>
    ///     Gets the height of the grid.
    /// </summary>
    int Height { get; }
    
    /// <summary>
    ///     Gets the total number of colors in the grid. This is equal to <see cref="Width"/> * <see cref="Height"/>.
    /// </summary>
    int Count { get; }
    
    /// <summary>
    ///     Gets the color at the specified index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    ChromaColor GetColor(int index);
    
    /// <summary>
    ///     Gets the colors in the grid. The length of the <paramref name="colors"/> span must be equal to or greater than <see cref="Count"/>. <para/>
    ///     Much faster than calling <see cref="GetColor(int)"/> for each index.
    /// </summary>
    /// <param name="colors"></param>
    void GetColors(Span<ChromaColor> colors);
}