using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace H.Generators;

[Generator]
public class ViewBaseGenerator : GeneratorBase, ISourceGenerator
{
    #region Methods

    public void Execute(GeneratorExecutionContext context)
    {
        try
        {
            var viewBases = context.AdditionalFiles
                .Where(text => GetOption(context, text, "BaseClass") != null)
                .Select(text => ViewBase.FromPath(
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

            if (viewBases.Any())
            {
                context.AddSource(
                    "ViewBase",
                    SourceText.From(
                        ViewBaseCodeGenerator.GenerateViewBases(
                            GetRequiredGlobalOption(context, "Namespace"),
                            viewBases),
                        Encoding.UTF8));
            }
            if (constructors.Any())
            {
                context.AddSource(
                    "Constructors",
                    SourceText.From(
                        ConstructorCodeGenerator.GenerateConstructors(
                            GetRequiredGlobalOption(context, "Namespace"),
                            constructors),
                        Encoding.UTF8));
            }
            if (defferedConstructors.Any())
            {
                context.AddSource(
                    "DefferedConstructors",
                    SourceText.From(
                        DefferedConstructorCodeGenerator.GenerateConstructors(
                            GetRequiredGlobalOption(context, "Namespace"),
                            defferedConstructors),
                        Encoding.UTF8));
            }
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
}
