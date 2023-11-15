using System;
using System.Runtime.CompilerServices;

namespace RazerSdkReader.Extensions;

internal static class IntExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToReadIndex(this uint writeIndex) => (int)(writeIndex - 1 < 9u ? writeIndex - 1 : 9u);

    public static DateTime ToDateTime(this ulong timestamp) => DateTime.Now.AddMilliseconds(-Environment.TickCount64).AddMilliseconds(timestamp);
}