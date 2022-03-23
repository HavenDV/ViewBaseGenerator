using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

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
                .Where(text => options.GetOption(text, $"{Name}_BaseClass") != null)
                .Select(text => ViewBase.FromPath(
                    text.Path,
                    options.GetOption(text, $"{Name}_Modifier") ?? "public",
                    options.GetOption(text, $"{Name}_BaseClass") ?? string.Empty,
                    options.GetOption(text, $"{Name}_ViewModelNamespace") ?? string.Empty))
                .ToArray();

            if (viewBases.Any())
            {
                context.AddTextSource(
                    "ViewBase",
                    ViewBaseCodeGenerator.GenerateViewBases(
                        options.GetRequiredGlobalOption($"{Name}_Namespace"),
                        viewBases));
            }
        }
        catch (Exception exception)
        {
            context.ReportException($"{Id}0001", exception);
        }
    }

    #endregion
}
