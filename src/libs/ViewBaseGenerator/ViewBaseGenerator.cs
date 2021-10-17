﻿using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace H.Generators;

[Generator]
public class ViewBaseGenerator : ISourceGenerator
{
    #region Methods

    public void Execute(GeneratorExecutionContext context)
    {
        try
        {
            var classes = context.AdditionalFiles
                .Select(text => ViewBaseClass.FromPath(
                    text.Path,
                    GetRequiredOption(context, text, "BaseClass"),
                    GetOption(context, text, "Modifier") ?? "public"))
                .ToArray();

            context.AddSource(
                "ViewBase",
                SourceText.From(
                    CodeGenerator.GenerateViewBaseClasses(
                        GetRequiredGlobalOption(context, "Namespace"),
                        classes),
                    Encoding.UTF8));
        }
        catch (Exception exception)
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
    }

    public void Initialize(GeneratorInitializationContext context)
    {
    }

    #endregion

    #region Utilities

    private static string? GetGlobalOption(GeneratorExecutionContext context, string name)
    {
        return context.AnalyzerConfigOptions.GlobalOptions.TryGetValue(
            $"build_property.{nameof(ViewBaseGenerator)}_{name}",
            out var result) &&
            !string.IsNullOrWhiteSpace(result)
            ? result
            : null;
    }

    private static string? GetOption(
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

    private static string GetRequiredGlobalOption(
        GeneratorExecutionContext context,
        string name)
    {
        return
            GetGlobalOption(context, name) ??
            throw new InvalidOperationException($"{name} is required.");
    }

    private static string GetRequiredOption(
        GeneratorExecutionContext context,
        AdditionalText text,
        string name)
    {
        return
            GetOption(context, text, name) ??
            throw new InvalidOperationException($"{name} is required.");
    }

    #endregion
}
