using Microsoft.CodeAnalysis;

namespace RazerSdkReader.Generators;

[Generator]
public class UnmanagedArrayAttributeGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            ctx.AddSource("UnmanagedArrayGenerator.UnmanagedArrayAttribute.g.cs", """
using System;

namespace UnmanagedArrayGenerator;

/// <summary>
/// Enables the generation of an unmanaged array.
/// </summary>
[AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
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
""");
        });
    }
}