using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using H.Generators.Extensions;

namespace H.Generators;

[Generator]
public class ViewBaseGenerator : IIncrementalGenerator
{
    #region Constants

    private const string Name = nameof(ViewBaseGenerator);
    private const string Id = "VBG";

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
            var suitableAdditionalTexts = additionalTexts
                .Where(text => options.GetOption(text, "GenerateViewBase", prefix: Name) != null)
                .ToArray();
            if (!suitableAdditionalTexts.Any())
            {
                return;
            }

            var @namespace = options.GetRequiredGlobalOption("Namespace", prefix: Name);
            var platform = options.TryRecognizePlatform(prefix: Name);
            var viewBases = suitableAdditionalTexts
                .Select(text => new ViewBase(
                    Namespace: @namespace,
                    Modifier: options.GetOption(text, nameof(ViewBase.Modifier), prefix: Name) ?? "public",
                    Name: Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(text.Path)).Replace("View", "ViewBase"),
                    BaseClass: options.GetRequiredOption(text, nameof(ViewBase.BaseClass), prefix: Name),
                    ViewModel: $"{options.GetRequiredOption(text, "ViewModelNamespace", prefix: Name)}.{Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(text.Path)).Replace("View", "ViewModel")}",
                    IsGeneric: bool.Parse(options.GetOption(text, nameof(ViewBase.IsGeneric), prefix: Name) ?? bool.FalseString),
                    IsAbstract: bool.Parse(options.GetOption(text, nameof(ViewBase.IsAbstract), prefix: Name) ?? bool.TrueString),
                    AddViewModelDependencyProperty: bool.Parse(options.GetOption(text, nameof(ViewBase.AddViewModelDependencyProperty), prefix: Name) ?? bool.FalseString),
                    Platform: platform))
                .ToArray();

            foreach (var viewBase in viewBases)
            {
                context.AddTextSource(
                    hintName: $"{viewBase.Name}.ViewBase.generated.cs",
                    text: ViewBaseCodeGenerator.GenerateViewBase(viewBase));
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
