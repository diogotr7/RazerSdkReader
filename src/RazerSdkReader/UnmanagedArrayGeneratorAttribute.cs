using System;

namespace RazerSdkReader;

[AttributeUsage(AttributeTargets.Struct)]
public class UnmanagedArrayAttribute : Attribute
{
    public Type Type { get; }
    public int Size { get; }
    
    public UnmanagedArrayAttribute(Type type, int size)
    {
        Type = type;
        Size = size;
    }
}