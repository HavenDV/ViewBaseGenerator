using Microsoft.CodeAnalysis;

namespace H.Generators;

[Generator]
public class ConstructorGenerator : ISourceGenerator
{
    #region Methods

    public void Execute(GeneratorExecutionContext context)
    {
        try
        {
            var constructors = context.AdditionalFiles
                .Where(text => context.GetOption(text, "GenerateConstructor") != null)
                .Select(text => Constructor.FromPath(
                    text.Path,
                    context.GetOption(text, "Modifier") ?? "public",
                    Convert.ToBoolean(context.GetOption(text, "SetReactiveUIDataContext") ?? bool.FalseString)))
                .ToArray();

            if (constructors.Any())
            {
                context.AddTextSource(
                    "Constructors",
                    ConstructorCodeGenerator.GenerateConstructors(
                        context.GetRequiredGlobalOption("Namespace"),
                        constructors));
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
