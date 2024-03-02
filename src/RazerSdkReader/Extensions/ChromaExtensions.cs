using System;
using System.Runtime.CompilerServices;

namespace RazerSdkReader.Extensions;

public static class IntExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToReadIndex(this uint writeIndex) => (int)Math.Min(writeIndex - 1, 9u);
}