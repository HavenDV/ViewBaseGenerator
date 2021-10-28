using Microsoft.CodeAnalysis;

namespace H.Generators;

[Generator]
public class DefferedConstructorGenerator : ISourceGenerator
{
    #region Methods

    public void Execute(GeneratorExecutionContext context)
    {
        try
        {
            var defferedConstructors = context.AdditionalFiles
                .Where(text => context.GetOption(text, "GenerateDefferedConstructor") != null)
                .Select(text => Constructor.FromPath(
                    text.Path,
                    context.GetOption(text, "Modifier") ?? "public",
                    false,
                    Convert.ToBoolean(context.GetOption(text, "SetReactiveUIDataContext") ?? bool.FalseString)))
                .ToArray();

            if (defferedConstructors.Any())
            {
                context.AddTextSource(
                    "DefferedConstructors",
                    DefferedConstructorCodeGenerator.GenerateConstructors(
                        context.GetRequiredGlobalOption("Namespace"),
                        defferedConstructors));
            }
        }
        catch (Exception exception)
        {
            context.ReportException(exception);
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
    }

    #endregion
}
