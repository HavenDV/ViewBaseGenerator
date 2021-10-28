using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace H.Generators;

public class GeneratorBase
{
    #region Methods

    protected static void AddSource(GeneratorExecutionContext context, string hintName, string text)
    {
        context.AddSource(
            hintName,
            SourceText.From(
                text,
                Encoding.UTF8));
    }

    protected static void ReportException(GeneratorExecutionContext context, Exception exception)
    {
        context.ReportDiagnostic(
            Diagnostic.Create(
                new DiagnosticDescriptor(
                    "VBG0001",
                    "Exception: ",
                    $"{exception}",
                    "Usage",
                    DiagnosticSeverity.Error,
                    true),
                Location.None));
    }

    protected static string? GetGlobalOption(GeneratorExecutionContext context, string name)
    {
        return context.AnalyzerConfigOptions.GlobalOptions.TryGetValue(
            $"build_property.{nameof(ViewBaseGenerator)}_{name}",
            out var result) &&
            !string.IsNullOrWhiteSpace(result)
            ? result
            : null;
    }

    protected static string? GetOption(
        GeneratorExecutionContext context,
        AdditionalText text,
        string name)
    {
        return context.AnalyzerConfigOptions.GetOptions(text).TryGetValue(
            $"build_metadata.AdditionalFiles.{nameof(ViewBaseGenerator)}_{name}",
            out var result) &&
            !string.IsNullOrWhiteSpace(result)
            ? result
            : null;
    }

    protected static string GetRequiredGlobalOption(
        GeneratorExecutionContext context,
        string name)
    {
        return
            GetGlobalOption(context, name) ??
            throw new InvalidOperationException($"{name} is required.");
    }

    #endregion
}
