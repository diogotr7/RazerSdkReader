using Microsoft.CodeAnalysis;

namespace RazerSdkReader.Generators;

public class DataOrDiagnostic<T>
{
    public DataOrDiagnostic(T data)
    {
        Data = data;
        Diagnostic = default;
    }

    public DataOrDiagnostic(Diagnostic diagnostic)
    {
        Diagnostic = diagnostic;
        Data = default;
    }

    public T? Data { get; }
    public Diagnostic? Diagnostic { get; }

    public static implicit operator DataOrDiagnostic<T>(T data) => new(data);
    public static implicit operator DataOrDiagnostic<T>(Diagnostic diag) => new(diag);
}