using System;

namespace RazerSdkReader.Extensions;

internal static class IntExtensions
{
    public static int FixIndex(this int writeIndex) => writeIndex switch
    {
        < 0 or > 9 => throw new ArgumentOutOfRangeException(nameof(writeIndex)),
        9 => 0,
        _ => writeIndex + 1
    };
}