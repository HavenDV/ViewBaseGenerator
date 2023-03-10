using System.Collections.Immutable;
using H.Generators.Tests.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace H.Generators.IntegrationTests;

public static class TestHelper
{
    public static async Task CheckSourceAsync<T>(
        this VerifyBase verifier,
        AnalyzerConfigOptionsProvider options,
        AdditionalText[] additionalTexts,
        bool addViewModels = true,
        bool addViewBases = false,
        CancellationToken cancellationToken = default) where T : IIncrementalGenerator, new()
    {
        var syntaxTrees = new List<SyntaxTree>();
        if (addViewModels)
        {
            syntaxTrees.Add(CSharpSyntaxTree.ParseText(@$"
namespace ViewModels
{{
{string.Concat(additionalTexts.Select(text => $@"
    public class {text.Path.Replace("View", "ViewModel").Replace(".xaml.cs", string.Empty)}
    {{
    }}
"))}
}}
", cancellationToken: cancellationToken));
        }
        if (addViewBases)
        {
            syntaxTrees.Add(CSharpSyntaxTree.ParseText(@$"
namespace Views
{{
{string.Concat(additionalTexts.Select(text => $@"
    public class {text.Path.Replace("View", "ViewBase").Replace(".xaml.cs", string.Empty)}
    {{
    }}
"))}
}}
", cancellationToken: cancellationToken));
        }
        var referenceAssemblies = ReferenceAssemblies.NetFramework.Net48.Wpf;
        var references = await referenceAssemblies.ResolveAsync(null, cancellationToken);
        var compilation = (Compilation)CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: syntaxTrees,
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var generator = new T();
        var driver = CSharpGeneratorDriver
            .Create(generator)
            .AddAdditionalTexts(ImmutableArray.Create(additionalTexts))
            .WithUpdatedAnalyzerConfigOptions(options)
            .RunGeneratorsAndUpdateCompilation(compilation, out compilation, out _, cancellationToken);
        var diagnostics = compilation.GetDiagnostics(cancellationToken);
        
        await Task.WhenAll(
            verifier
                .Verify(diagnostics.NormalizeLocations())
                .UseDirectory("Snapshots")
                .UseTextForParameters("Diagnostics"),
            verifier
                .Verify(driver)
                .UseDirectory("Snapshots"));
    }
}