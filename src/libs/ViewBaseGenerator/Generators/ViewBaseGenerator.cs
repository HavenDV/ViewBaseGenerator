using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using H.Generators.Extensions;

namespace H.Generators;

[Generator]
public class ViewBaseGenerator : IIncrementalGenerator
{
    #region Constants

    public const string Name = nameof(ViewBaseGenerator);
    public const string Id = "VBG";

    #endregion

    #region Methods

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.CompilationProvider
                .Combine(context.AnalyzerConfigOptionsProvider)
                .Combine(context.AdditionalTextsProvider.Collect()),
            static (context, tuple) => Execute(tuple.Left.Right, tuple.Right, context));
    }

    private static void Execute(
        AnalyzerConfigOptionsProvider options,
        ImmutableArray<AdditionalText> additionalTexts,
        SourceProductionContext context)
    {
        try
        {
            var viewBases = additionalTexts
                .Where(text => options.GetOption(text, "BaseClass", prefix: Name) != null)
                .Select(text => ViewBase.FromPath(
                    text.Path,
                    options.GetOption(text, "Modifier", prefix: Name) ?? "public",
                    options.GetOption(text, "BaseClass", prefix: Name) ?? string.Empty,
                    options.GetOption(text, "ViewModelNamespace", prefix: Name) ?? string.Empty))
                .ToArray();

            foreach (var viewBase in viewBases)
            {
                context.AddTextSource(
                    hintName: $"{viewBase.Name}.ViewBase.generated.cs",
                    text: ViewBaseCodeGenerator.GenerateViewBase(
                        options.GetRequiredGlobalOption("Namespace", prefix: Name),
                        viewBase));
            }
        }
        catch (Exception exception)
        {
            context.ReportException(
                id: "001",
                exception: exception,
                prefix: Id);
        }
    }

    #endregion
}
