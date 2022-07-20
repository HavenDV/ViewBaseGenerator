using Microsoft.CodeAnalysis;
using H.Generators.Tests.Extensions;

namespace H.Generators.IntegrationTests;

[TestClass]
public class ConstructorTests : VerifyBase
{
    [TestMethod]
    public Task GeneratesWithoutFilesCorrectly()
    {
        return this.CheckSourceAsync<ConstructorGenerator>(
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

        return this.CheckSourceAsync<ConstructorGenerator>(
            options,
            new AdditionalText[] { new MemoryAdditionalText("TestView.xaml.cs", "") });
    }

    [TestMethod]
    public Task GeneratesWithOneFileCorrectly()
    {
        var options = new DictionaryAnalyzerConfigOptionsProvider(
            globalOptions: new Dictionary<string, string>
            {
                ["build_property.ConstructorGenerator_Namespace"] = "Views",
            },
            additionalTextOptions: new Dictionary<string, Dictionary<string, string>>
            {
                ["TestView.xaml.cs"] = new()
                {
                    ["build_metadata.AdditionalFiles.ConstructorGenerator_GenerateConstructor"] = "true",
                },
            });

        return this.CheckSourceAsync<ConstructorGenerator>(
            options,
            new AdditionalText[] { new MemoryAdditionalText("TestView.xaml.cs", "") });
    }

    [TestMethod]
    public Task InheritFromViewBase()
    {
        var options = new DictionaryAnalyzerConfigOptionsProvider(
            globalOptions: new Dictionary<string, string>
            {
                ["build_property.ConstructorGenerator_Namespace"] = "Views",
            },
            additionalTextOptions: new Dictionary<string, Dictionary<string, string>>
            {
                ["TestView.xaml.cs"] = new()
                {
                    ["build_metadata.AdditionalFiles.ConstructorGenerator_GenerateConstructor"] = "true",
                    ["build_metadata.AdditionalFiles.ConstructorGenerator_InheritFromViewBase"] = "true",
                },
            });

        return this.CheckSourceAsync<ConstructorGenerator>(
            options,
            new AdditionalText[] { new MemoryAdditionalText("TestView.xaml.cs", "") },
            addViewBases: true);
    }
}