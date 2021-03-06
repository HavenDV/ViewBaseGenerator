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
                    Modifier: options.GetOption(text, "Modifier", prefix: Name) ?? "public",
                    Name: Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(text.Path)).Replace("View", "ViewBase"),
                    BaseClass: options.GetRequiredOption(text, "BaseClass", prefix: Name) ?? string.Empty,
                    ViewModel: $"{options.GetRequiredOption(text, "ViewModelNamespace", prefix: Name) ?? string.Empty}.{Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(text.Path)).Replace("View", "ViewModel")}",
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
