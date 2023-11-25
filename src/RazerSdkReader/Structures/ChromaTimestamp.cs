using System;

namespace RazerSdkReader.Structures;

public readonly record struct ChromaTimestamp
{
    public readonly ulong TickCount64;
    public DateTime ToDateTime() => DateTime.Now.AddMilliseconds(-Environment.TickCount64).AddMilliseconds(TickCount64);
    public override string ToString() => ToDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
    public static implicit operator DateTime(ChromaTimestamp timestamp) => timestamp.ToDateTime();
}