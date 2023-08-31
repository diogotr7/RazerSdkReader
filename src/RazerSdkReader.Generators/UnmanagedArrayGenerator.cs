using System.Threading;
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
public class UnmanagedArrayGenerator : IIncrementalGenerator
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

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var input = context.SyntaxProvider.CreateSyntaxProvider(IsAttributePresent, GetStructInfo);

        context.RegisterSourceOutput(input,
            (spc, structInfo) =>
            {
                spc.AddSource($"{structInfo.ParentStruct}.g.cs", StructGenerator.Generate(structInfo));
            });
    }

    private static bool IsAttributePresent(SyntaxNode syntaxNode, CancellationToken token)
    {
        if (syntaxNode is not AttributeSyntax attribute)
            return false;

        var name = attribute.Name switch
        {
            SimpleNameSyntax ins => ins.Identifier.Text,
            QualifiedNameSyntax qns => qns.Right.Identifier.Text,
            _ => null
        };

        return name is "UnmanagedArray" or "UnmanagedArrayAttribute";
    }

    private static bool IsStructOrRecordStruct(SyntaxNode node) => node switch
    {
        StructDeclarationSyntax => true,
        RecordDeclarationSyntax rds => rds.Kind() == SyntaxKind.RecordStructDeclaration,
        _ => false
    };

    private static StructInfo GetStructInfo(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        var structAttribute = (AttributeSyntax)context.Node;

        // "attribute.Parent" is "AttributeListSyntax"
        // "attribute.Parent.Parent" is a C# fragment the attributes are applied to
        if (!IsStructOrRecordStruct(structAttribute.Parent?.Parent))
            return null;

        var structDeclarationSyntax = (TypeDeclarationSyntax)structAttribute.Parent!.Parent;
        if (structDeclarationSyntax is null)
            return null;

        var parentType = context.SemanticModel.GetDeclaredSymbol(structDeclarationSyntax);

        var structParameters = structAttribute.ArgumentList!.Arguments;
        if (structParameters.Count != 2)
            return null;

        var childType = context.SemanticModel.GetTypeInfo((structParameters[0].Expression as TypeOfExpressionSyntax)!.Type);
        var childCountExpression = context.SemanticModel.GetConstantValue(structParameters[1].Expression);
        if (childCountExpression.Value is not int childCount)
            return null;

        var format = new SymbolDisplayFormat(miscellaneousOptions: SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers);
        return new StructInfo
        {
            Namespace = parentType!.ContainingNamespace.ToString(),
            ParentStruct = parentType!.ToDisplayString(format),
            ChildStruct = childType!.Type!.ToDisplayString(format),
            Count = childCount
        };
    }
}