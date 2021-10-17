using System.Text;
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
            var classes = new List<ViewBaseClass>();
            var @namespace =
                GetGlobalOption(context, "Namespace") ??
                throw new InvalidOperationException("Namespace is required.");

            context.AddSource(
                "ViewBase",
                SourceText.From(
                    CodeGenerator.GenerateViewBaseClasses(
                        @namespace,
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

    //private static string? GetOption(
    //    GeneratorExecutionContext context, 
    //    string name, 
    //    AdditionalText text)
    //{
    //    return context.AnalyzerConfigOptions.GetOptions(text).TryGetValue(
    //        $"build_metadata.AdditionalFiles.{nameof(HResourcesGenerator)}_{name}", 
    //        out var result) &&
    //        !string.IsNullOrWhiteSpace(result)
    //        ? result
    //        : null;
    //}

    #endregion
}
