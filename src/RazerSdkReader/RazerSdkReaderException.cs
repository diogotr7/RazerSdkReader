using System;

namespace RazerSdkReader;

public class RazerSdkReaderException : Exception
{
    public RazerSdkReaderException(string message, Exception innerException) : base(message, innerException)
    {
    }
}