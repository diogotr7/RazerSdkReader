using System;

namespace RazerSdkReader.Extensions;

internal static class IntExtensions
{
    /// <summary>
    ///     The write index is a flag for the sdk to know when the next frame should be written.
    ///     This means that the read index is always one frame behind the write index.
    ///     This method converts the write index to the read index, wrapping around if necessary.
    /// </summary>
    public static int ToReadIndex(this int writeIndex) => writeIndex switch
    {
        < 0 or > 9 => throw new ArgumentOutOfRangeException(nameof(writeIndex)),
        0 => 9,
        _ => writeIndex - 1
    };
    
    public static DateTime ToDateTime(this ulong timestamp) => DateTime.Now.AddMilliseconds(-Environment.TickCount64).AddMilliseconds(timestamp);
}