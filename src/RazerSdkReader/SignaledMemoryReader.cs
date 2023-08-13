using System;
using System.Runtime.Versioning;

namespace RazerSdkReader;

[SupportedOSPlatform("windows")]
internal sealed class SignaledMemoryReader<T> : SignaledReader where T : unmanaged
{
    private readonly MemoryMappedStructReader<T> _reader;

    public event EventHandler<T>? Updated;

    internal SignaledMemoryReader(string mmf, string eventWaitHandle) : base(eventWaitHandle)
    {
        _reader = new MemoryMappedStructReader<T>(mmf);
    }

    internal override void Update()
    {
        Updated?.Invoke(this, _reader.Read());
        EventWaitHandle.Reset();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        _reader.Dispose();
    }
}