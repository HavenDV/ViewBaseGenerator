using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

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
                .Where(text => options.GetOption(text, $"{Name}_GenerateConstructor") != null)
                .Select(text => Constructor.FromPath(
                    text.Path,
                    options.GetOption(text, $"{Name}_Modifier") ?? "public",
                    Convert.ToBoolean(options.GetOption(text, $"{Name}_SetReactiveUIDataContext") ?? bool.FalseString)))
                .ToArray();

            if (constructors.Any())
            {
                context.AddTextSource(
                    "Constructors",
                    ConstructorCodeGenerator.GenerateConstructors(
                        options.GetRequiredGlobalOption($"{Name}_Namespace"),
                        constructors));
            }
        }
        catch (Exception exception)
        {
            context.ReportException($"{Id}0001", exception);
        }
    }

    #endregion
}
