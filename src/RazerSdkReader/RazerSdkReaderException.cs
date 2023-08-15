using System;

namespace RazerSdkReader;

public class RazerSdkReaderException : Exception
{
    public RazerSdkReaderException()
    {
    }

    public RazerSdkReaderException(string message) : base(message)
    {
    }

    public RazerSdkReaderException(string message, Exception innerException) : base(message, innerException)
    {
    }
}