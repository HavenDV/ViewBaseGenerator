using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace H.Generators.IntegrationTests;

internal class CustomAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider
{
    public override AnalyzerConfigOptions GlobalOptions { get; }
    public Dictionary<string, Dictionary<string, string>> TreeOptions { get; }
    public Dictionary<string, Dictionary<string, string>> AdditionalTextOptions { get; }

    public CustomAnalyzerConfigOptionsProvider(
        Dictionary<string, string>? globalOptions = null,
        Dictionary<string, Dictionary<string, string>>? treeOptions = null,
        Dictionary<string, Dictionary<string, string>>? additionalTextOptions = null)
    {
        GlobalOptions = new CustomAnalyzerConfigOptions(globalOptions ?? new());
        TreeOptions = treeOptions ?? new();
        AdditionalTextOptions = additionalTextOptions ?? new();
    }

    public override AnalyzerConfigOptions GetOptions(SyntaxTree tree)
    {
        return new CustomAnalyzerConfigOptions(TreeOptions[tree.FilePath]);
    }

    public override AnalyzerConfigOptions GetOptions(AdditionalText textFile)
    {
        return new CustomAnalyzerConfigOptions(AdditionalTextOptions[textFile.Path]);
    }
}
