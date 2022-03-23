using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace H.Generators;

internal static class GeneratorExtensions
{
    #region Methods

    public static void AddTextSource(
        this SourceProductionContext context,
        string hintName,
        string text,
        Encoding? encoding = null)
    {
        context.AddSource(
            hintName,
            SourceText.From(
                text,
                encoding ?? Encoding.UTF8));
    }

    public static void ReportException(
        this SourceProductionContext context,
        string id,
        Exception exception)
    {
        context.ReportDiagnostic(
            Diagnostic.Create(
                new DiagnosticDescriptor(
                    id,
                    "Exception: ",
                    $"{exception}",
                    "Usage",
                    DiagnosticSeverity.Error,
                    true),
                Location.None));
    }

    public static string? GetGlobalOption(
        this AnalyzerConfigOptionsProvider provider,
        string name)
    {
        return provider.GlobalOptions.TryGetValue(
            $"build_property.{name}",
            out var result) &&
            !string.IsNullOrWhiteSpace(result)
            ? result
            : null;
    }

    public static string? GetOption(
        this AnalyzerConfigOptionsProvider provider,
        AdditionalText text,
        string name)
    {
        return provider.GetOptions(text).TryGetValue(
            $"build_metadata.AdditionalFiles.{name}",
            out var result) &&
            !string.IsNullOrWhiteSpace(result)
            ? result
            : null;
    }

    public static string GetRequiredGlobalOption(
        this AnalyzerConfigOptionsProvider provider,
        string name)
    {
        return
            provider.GetGlobalOption(name) ??
            throw new InvalidOperationException($"{name} is required.");
    }

    #endregion
}
