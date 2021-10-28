using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace H.Generators;

public static class GeneratorExtensions
{
    #region Constants

    public const string Name = nameof(ViewBaseGenerator);
    public const string Id = "VBG";

    #endregion

    #region Methods

    public static void AddTextSource(
        this GeneratorExecutionContext context,
        string hintName,
        string text)
    {
        context.AddSource(
            hintName,
            SourceText.From(
                text,
                Encoding.UTF8));
    }

    public static void ReportException(
        this GeneratorExecutionContext context,
        Exception exception)
    {
        context.ReportDiagnostic(
            Diagnostic.Create(
                new DiagnosticDescriptor(
                    $"{Id}0001",
                    "Exception: ",
                    $"{exception}",
                    "Usage",
                    DiagnosticSeverity.Error,
                    true),
                Location.None));
    }

    public static string? GetGlobalOption(
        this GeneratorExecutionContext context,
        string name)
    {
        return context.AnalyzerConfigOptions.GlobalOptions.TryGetValue(
            $"build_property.{Name}_{name}",
            out var result) &&
            !string.IsNullOrWhiteSpace(result)
            ? result
            : null;
    }

    public static string? GetOption(
        this GeneratorExecutionContext context,
        AdditionalText text,
        string name)
    {
        return context.AnalyzerConfigOptions.GetOptions(text).TryGetValue(
            $"build_metadata.AdditionalFiles.{Name}_{name}",
            out var result) &&
            !string.IsNullOrWhiteSpace(result)
            ? result
            : null;
    }

    public static string GetRequiredGlobalOption(
        this GeneratorExecutionContext context,
        string name)
    {
        return
            GetGlobalOption(context, name) ??
            throw new InvalidOperationException($"{name} is required.");
    }

    #endregion
}
