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
            var classes = context.AdditionalFiles
                .Where(text => GetOption(context, text, "BaseClass") != null)
                .Select(text => ViewBaseClass.FromPath(
                    text.Path,
                    GetOption(context, text, "Modifier") ?? "public",
                    GetOption(context, text, "BaseClass") ?? string.Empty,
                    GetOption(context, text, "ViewModelNamespace") ?? string.Empty))
                .ToArray();
            var constructors = context.AdditionalFiles
                .Where(text => GetOption(context, text, "GenerateConstructor") != null)
                .Select(text => Constructor.FromPath(
                    text.Path,
                    GetOption(context, text, "Modifier") ?? "public",
                    Convert.ToBoolean(GetOption(context, text, "IsDeffered") ?? bool.FalseString),
                    Convert.ToBoolean(GetOption(context, text, "SetReactiveUIDataContext") ?? bool.FalseString)))
                .ToArray();
            var defferedConstructors = context.AdditionalFiles
                .Where(text => GetOption(context, text, "GenerateDefferedConstructor") != null)
                .Select(text => Constructor.FromPath(
                    text.Path,
                    GetOption(context, text, "Modifier") ?? "public",
                    false,
                    Convert.ToBoolean(GetOption(context, text, "SetReactiveUIDataContext") ?? bool.FalseString)))
                .ToArray();

            context.AddSource(
                "ViewBase",
                SourceText.From(
                    CodeGenerator.GenerateViewBaseClasses(
                        GetRequiredGlobalOption(context, "Namespace"),
                        classes),
                    Encoding.UTF8));
            context.AddSource(
                "Constructors",
                SourceText.From(
                    ConstructorCodeGenerator.GenerateConstructors(
                        GetRequiredGlobalOption(context, "Namespace"),
                        constructors),
                    Encoding.UTF8));
            context.AddSource(
                "DefferedConstructors",
                SourceText.From(
                    DefferedConstructorCodeGenerator.GenerateConstructors(
                        GetRequiredGlobalOption(context, "Namespace"),
                        defferedConstructors),
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

    #endregion
}
