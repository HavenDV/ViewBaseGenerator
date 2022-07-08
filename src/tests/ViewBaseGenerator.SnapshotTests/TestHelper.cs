using System.Collections.Immutable;
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
        CancellationToken cancellationToken = default) where T : IIncrementalGenerator, new()
    {
        var referenceAssemblies = ReferenceAssemblies.NetFramework.Net48.Wpf
            .WithPackages(ImmutableArray.Create(new PackageIdentity("ReactiveUI.WPF", "18.2.9")));
        var references = await referenceAssemblies.ResolveAsync(null, cancellationToken);
        var compilation = (Compilation)CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: new[]
            {
                CSharpSyntaxTree.ParseText(@$"
namespace ViewModels
{{
{string.Concat(additionalTexts.Select(text => $@"
    public class {text.Path.Replace("View", "ViewModel").Replace(".xaml.cs", string.Empty)}
    {{
    }}
"))}
}}
", cancellationToken: cancellationToken),
            },
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
                .Verify(diagnostics)
                .UseDirectory("Snapshots")
                .UseTextForParameters("Diagnostics"),
            verifier
                .Verify(driver)
                .UseDirectory("Snapshots"));
    }
}