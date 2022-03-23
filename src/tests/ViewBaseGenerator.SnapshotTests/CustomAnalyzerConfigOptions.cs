using Microsoft.CodeAnalysis.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace H.Generators.IntegrationTests;

internal sealed class CustomAnalyzerConfigOptions : AnalyzerConfigOptions
{
    private Dictionary<string, string> Properties { get; }

    public CustomAnalyzerConfigOptions(Dictionary<string, string> properties)
    {
        Properties = properties;
    }

    public override bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
    {
        return Properties.TryGetValue(key, out value);
    }
}