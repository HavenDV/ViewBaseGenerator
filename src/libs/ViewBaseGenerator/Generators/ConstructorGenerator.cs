using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using H.Generators.Extensions;

namespace H.Generators;

[Generator]
public class ConstructorGenerator : IIncrementalGenerator
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
            var constructors = additionalTexts
                .Where(text => options.GetOption(text, "GenerateConstructor", prefix: Name) != null)
                .Select(text => new Constructor(
                    Modifier: options.GetOption(text, "Modifier", prefix: Name) ?? "public",
                    Name: Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(text.Path)),
                    CreateReactiveUIWhenActivated: bool.Parse(options.GetOption(text, "CreateReactiveUIWhenActivated", prefix: Name) ?? bool.FalseString),
                    SetReactiveUIDataContext: bool.Parse(options.GetOption(text, "SetReactiveUIDataContext", prefix: Name) ?? bool.FalseString)))
                .ToArray();

            foreach (var constructor in constructors)
            {
                context.AddTextSource(
                    hintName: $"{constructor.Name}.Constructors.generated.cs",
                    text: ConstructorCodeGenerator.GenerateConstructor(
                        options.GetRequiredGlobalOption("Namespace", prefix: Name),
                        constructor));
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
