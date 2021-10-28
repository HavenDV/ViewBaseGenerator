using Microsoft.CodeAnalysis;

namespace H.Generators;

[Generator]
public class ViewBaseGenerator : ISourceGenerator
{
    #region Methods

    public void Execute(GeneratorExecutionContext context)
    {
        try
        {
            var viewBases = context.AdditionalFiles
                .Where(text => context.GetOption(text, "BaseClass") != null)
                .Select(text => ViewBase.FromPath(
                    text.Path,
                    context.GetOption(text, "Modifier") ?? "public",
                    context.GetOption(text, "BaseClass") ?? string.Empty,
                    context.GetOption(text, "ViewModelNamespace") ?? string.Empty))
                .ToArray();

            if (viewBases.Any())
            {
                context.AddTextSource(
                    "ViewBase",
                    ViewBaseCodeGenerator.GenerateViewBases(
                        context.GetRequiredGlobalOption("Namespace"),
                        viewBases));
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
