using Microsoft.CodeAnalysis;
using H.Generators.Tests.Extensions;

namespace H.Generators.IntegrationTests;

[TestClass]
public class ViewBaseGeneratorSnapshotTests : VerifyBase
{
    [TestMethod]
    public Task GeneratesWithoutFilesCorrectly()
    {
        return this.CheckSourceAsync(
            new DictionaryAnalyzerConfigOptionsProvider(),
            Array.Empty<AdditionalText>());
    }

    [TestMethod]
    public Task GeneratesWithOneFileWithoutSettingsCorrectly()
    {
        var options = new DictionaryAnalyzerConfigOptionsProvider(
            additionalTextOptions: new Dictionary<string, Dictionary<string, string>>
            {
                ["TestView.xaml.cs"] = new(),
            });

        return this.CheckSourceAsync(
            options,
            new AdditionalText[] { new MemoryAdditionalText("TestView.xaml.cs", "") });
    }

    [TestMethod]
    public Task GeneratesWithOneFileCorrectly()
    {
        var options = new DictionaryAnalyzerConfigOptionsProvider(
            globalOptions: new Dictionary<string, string>
            {
                ["build_property.ViewBaseGenerator_Namespace"] = "Views",
            },
            additionalTextOptions: new Dictionary<string, Dictionary<string, string>>
            {
                ["TestView.xaml.cs"] = new()
                {
                    ["build_metadata.AdditionalFiles.ViewBaseGenerator_BaseClass"] = "global::System.Collections.Generic.List",
                    ["build_metadata.AdditionalFiles.ViewBaseGenerator_ViewModelNamespace"] = "ViewModels",
                },
            });

        return this.CheckSourceAsync(
            options,
            new AdditionalText[] { new MemoryAdditionalText("TestView.xaml.cs", "") });
    }
}