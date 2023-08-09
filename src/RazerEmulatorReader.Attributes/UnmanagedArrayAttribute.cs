﻿using System;

namespace RazerEmulatorReader.Attributes;

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