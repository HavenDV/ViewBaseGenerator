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
                .Where(text => options.GetOption(text, "GenerateConstructor", prefix: Name) != null)
                .ToArray();
            if (!suitableAdditionalTexts.Any())
            {
                return;
            }

            var @namespace = options.GetRequiredGlobalOption("Namespace", prefix: Name);
            var platform = options.TryRecognizePlatform(prefix: Name);
            var constructors = suitableAdditionalTexts
                .Select(text => new Constructor(
                    Namespace: @namespace,
                    Modifier: options.GetOption(text, "Modifier", prefix: Name) ?? "public",
                    Name: Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(text.Path)),
                    InheritFromViewBase: bool.Parse(options.GetOption(text, nameof(Constructor.InheritFromViewBase), prefix: Name) ?? bool.FalseString),
                    BaseClass: options.GetOption(text, "BaseClass", prefix: Name) ?? string.Empty,
                    Platform: platform))
                .ToArray();

            foreach (var constructor in constructors)
            {
                context.AddTextSource(
                    hintName: $"{constructor.Name}.Constructors.generated.cs",
                    text: ConstructorCodeGenerator.GenerateConstructor(constructor));
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
