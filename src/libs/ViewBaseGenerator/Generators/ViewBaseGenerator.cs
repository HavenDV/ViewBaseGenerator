using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
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
        context.AdditionalTextsProvider
            .Combine(context.AnalyzerConfigOptionsProvider)
            .Where(static x => x.Right.GetOption(x.Left, "GenerateViewBase", prefix: Name) != null)
            .SelectAndReportExceptions(PrepareData, context, Id)
            .SelectAndReportExceptions(GetSource, context, Id)
            .AddSource(context);
    }

    private static ViewBase PrepareData(
        (AdditionalText Text, AnalyzerConfigOptionsProvider Options) tuple)
    {
        var (text, options) = tuple;
        
        return new ViewBase(
            Namespace: options.GetRequiredGlobalOption("Namespace", prefix: Name),
            Modifier: options.GetOption(text, nameof(ViewBase.Modifier), prefix: Name) ?? "public",
            Name: Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(text.Path))
                .Replace("View", "ViewBase"),
            BaseClass: options.GetOption(text, nameof(ViewBase.BaseClass), prefix: Name) ?? string.Empty,
            ViewModel:
            $"{options.GetOption(text, "ViewModelNamespace", prefix: Name) ?? string.Empty}.{Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(text.Path)).Replace("View", "ViewModel")}",
            IsGeneric: bool.Parse(options.GetOption(text, nameof(ViewBase.IsGeneric), prefix: Name) ??
                                  bool.FalseString),
            IsAbstract: bool.Parse(
                options.GetOption(text, nameof(ViewBase.IsAbstract), prefix: Name) ?? bool.TrueString),
            AddViewModelDependencyProperty: bool.Parse(
                options.GetOption(text, nameof(ViewBase.AddViewModelDependencyProperty), prefix: Name) ??
                bool.FalseString),
            Framework: options.TryRecognizeFramework());
    }

    private static FileWithName GetSource(ViewBase viewBase)
    {
        return new FileWithName(
            Name: $"{viewBase.Name}.ViewBase.generated.cs",
            Text: ViewBaseCodeGenerator.GenerateViewBase(viewBase));
    }

    #endregion
}
