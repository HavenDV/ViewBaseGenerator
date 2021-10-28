using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace H.Generators;

[Generator]
public class DefferedConstructorGenerator : GeneratorBase, ISourceGenerator
{
    #region Methods

    public void Execute(GeneratorExecutionContext context)
    {
        try
        {
            var defferedConstructors = context.AdditionalFiles
                .Where(text => GetOption(context, text, "GenerateDefferedConstructor") != null)
                .Select(text => Constructor.FromPath(
                    text.Path,
                    GetOption(context, text, "Modifier") ?? "public",
                    false,
                    Convert.ToBoolean(GetOption(context, text, "SetReactiveUIDataContext") ?? bool.FalseString)))
                .ToArray();

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
            ReportException(context, exception);
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
    }

    #endregion
}
