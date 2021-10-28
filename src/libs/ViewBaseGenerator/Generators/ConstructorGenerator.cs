using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace H.Generators;

[Generator]
public class ConstructorGenerator : GeneratorBase, ISourceGenerator
{
    #region Methods

    public void Execute(GeneratorExecutionContext context)
    {
        try
        {
            var constructors = context.AdditionalFiles
                .Where(text => GetOption(context, text, "GenerateConstructor") != null)
                .Select(text => Constructor.FromPath(
                    text.Path,
                    GetOption(context, text, "Modifier") ?? "public",
                    Convert.ToBoolean(GetOption(context, text, "IsDeffered") ?? bool.FalseString),
                    Convert.ToBoolean(GetOption(context, text, "SetReactiveUIDataContext") ?? bool.FalseString)))
                .ToArray();

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
