using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace H.Generators.IntegrationTests;

public static class TestHelper
{
    public static async Task CheckSource(
        this VerifyBase verifier,
        AnalyzerConfigOptionsProvider options,
        params AdditionalText[] texts)
    {
        var dotNetFolder = Path.GetDirectoryName(typeof(object).Assembly.Location) ?? string.Empty;
        var compilation = (Compilation)CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: new[]
            {
                CSharpSyntaxTree.ParseText(@$"
namespace ViewModels
{{
{string.Concat(texts.Select(text => $@"
    public class {text.Path.Replace("View", "ViewModel").Replace(".xaml.cs", string.Empty)}
    {{
    }}
"))}
}}
"),
                CSharpSyntaxTree.ParseText(@"
namespace MyCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
        }
    }
}
"),
            },
            references: new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(Path.Combine(dotNetFolder, "System.Runtime.dll")),
                MetadataReference.CreateFromFile(Path.Combine(dotNetFolder, "System.Collections.dll")),
                MetadataReference.CreateFromFile(Path.Combine(dotNetFolder, "netstandard.dll")),
            });
        var generator = new ViewBaseGenerator();
        var driver = CSharpGeneratorDriver
            .Create(generator)
            .AddAdditionalTexts(ImmutableArray.Create(texts));
        driver = driver
            .WithUpdatedAnalyzerConfigOptions(options)
            .RunGenerators(compilation);
        
        driver = driver.RunGeneratorsAndUpdateCompilation(
            compilation,
            out compilation,
            out _);
        var diagnostics = compilation.GetDiagnostics();

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