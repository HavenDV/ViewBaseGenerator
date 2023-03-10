using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using H.Generators.Extensions;

namespace H.Generators;

[Generator]
public class ConstructorGenerator : IIncrementalGenerator
{
    #region Constants

    private const string Name = nameof(ConstructorGenerator);
    private const string Id = "CG";

    #endregion

    #region Methods

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.AdditionalTextsProvider
            .Combine(context.AnalyzerConfigOptionsProvider)
            .Where(static x => x.Right.GetOption(x.Left, "GenerateConstructor", prefix: Name) != null)
            .SelectAndReportExceptions(PrepareData, context, Id)
            .SelectAndReportExceptions(GetSource, context, Id)
            .AddSource(context);
    }

    private static Constructor PrepareData(
        (AdditionalText Text, AnalyzerConfigOptionsProvider Options) tuple)
    {
        var (text, options) = tuple;
        
        var @namespace = options.GetRequiredGlobalOption("Namespace", prefix: Name);
        var framework = options.TryRecognizeFramework();
            
        return new Constructor(
            Namespace: @namespace,
            Modifier: options.GetOption(text, "Modifier", prefix: Name) ?? "public",
            Name: Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(text.Path)),
            InheritFromViewBase: bool.Parse(
                options.GetOption(text, nameof(Constructor.InheritFromViewBase), prefix: Name) ?? bool.FalseString),
            BaseClass: options.GetOption(text, "BaseClass", prefix: Name) ?? string.Empty,
            Framework: framework);
    }

    private static FileWithName GetSource(Constructor constructor)
    {
        return new FileWithName(
            Name: $"{constructor.Name}.Constructors.generated.cs",
            Text: ConstructorCodeGenerator.GenerateConstructor(constructor));
    }

    #endregion
}
