using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RazerSdkReader.Generators;

/// <summary>
///     Source generator used to expand structs into unmanaged-compatible ones.
///     This means instead of using an array internally, or a fixed buffer,
///     we stack X times the struct manually with fields, then add an operator to access them by index.
/// </summary>
[Generator]
public class UnmanagedArrayGenerator : ISourceGenerator
{
    //Example:
    //[UnmanagedArray(typeof(ChildTestStruct), 2)]
    //public readonly partial record struct TestStruct;

    //Generates:
    //public readonly partial record struct TestStruct
    //{
    //    public ChildTestStruct Child0;
    //    public ChildTestStruct Child1;
    //
    //    public ChildTestStruct this[int index] => index switch
    //    {
    //        0 => Child0,
    //        1 => Child1,
    //        _ => throw new IndexOutOfRangeException()
    //    };
    //}

    public void Initialize(GeneratorInitializationContext context)
    {
        //empty
    }

    public void Execute(GeneratorExecutionContext context)
    {
        List<StructInfo> structInfos = new();

        var structs = context.Compilation.SyntaxTrees
            .SelectMany(st => st.GetRoot()
                .DescendantNodes()
                .OfType<RecordDeclarationSyntax>()
                .Where(r => r.AttributeLists
                    .SelectMany(al => al.Attributes)
                    .Any(a => a.Name.GetText().ToString() == "UnmanagedArray")));

        foreach (var s in structs)
        {
            var tree = s.SyntaxTree;
            var model = context.Compilation.GetSemanticModel(tree);

            var structAttribute = s.AttributeLists
                .SelectMany(al => al.Attributes)
                .First(a => a.Name.GetText().ToString() == "UnmanagedArray");

            var structParameters = structAttribute.ArgumentList!.Arguments;

            var childType = model.GetTypeInfo((structParameters[0].Expression as TypeOfExpressionSyntax)!.Type);
            var childCountExpression = model.GetConstantValue(structParameters[1].Expression);
            if (childCountExpression.Value is not int childCount)
            {
                //diag
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor(
                        "RAZSDK001",
                        "Invalid child count",
                        "Invalid child count",
                        "RazerSdkReader",
                        DiagnosticSeverity.Error,
                        true),
                    Location.None));
                return;
            }

            var parentType = model.GetDeclaredSymbol(s);

            structInfos.Add(new StructInfo
            {
                Namespace = parentType!.ContainingNamespace.ToString(),
                ParentStruct = parentType!.ToString().Split('.').Last(),
                ChildStruct = childType!.Type!.ToString().Split('.').Last(),
                Count = childCount
            });
        }

        foreach (var structInfo in structInfos)
        {
            context.AddSource($"{structInfo.ParentStruct}.g.cs", StructGenerator.Generate(structInfo));
        }
    }
}