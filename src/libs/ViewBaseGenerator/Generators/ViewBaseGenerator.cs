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
