namespace H.Generators.IntegrationTests;

[TestClass]
public class ViewBaseGeneratorSnapshotTests : VerifyBase
{
    [TestMethod]
    public Task GeneratesWithoutFilesCorrectly()
    {
        return this.CheckSource(
            new CustomAnalyzerConfigOptionsProvider());
    }

    [TestMethod]
    public Task GeneratesWithOneFileWithoutSettingsCorrectly()
    {
        var options = new CustomAnalyzerConfigOptionsProvider(
            additionalTextOptions: new Dictionary<string, Dictionary<string, string>>
            {
                { "TestView.xaml.cs", new() },
            });

        return this.CheckSource(
            options,
            new CustomAdditionalText("TestView.xaml.cs", ""));
    }

    [TestMethod]
    public Task GeneratesWithOneFileCorrectly()
    {
        var options = new CustomAnalyzerConfigOptionsProvider(
            globalOptions: new Dictionary<string, string>
            {
                { "build_property.ViewBaseGenerator_Namespace", "Views" },
            },
            additionalTextOptions: new Dictionary<string, Dictionary<string, string>>
            {
                {
                    "TestView.xaml.cs",
                    new Dictionary<string, string>
                    {
                        { "build_metadata.AdditionalFiles.ViewBaseGenerator_BaseClass", "global::System.Collections.Generic.List" },
                        { "build_metadata.AdditionalFiles.ViewBaseGenerator_ViewModelNamespace", "ViewModels" },
                    }
                },
            });

        return this.CheckSource(
            options,
            new CustomAdditionalText("TestView.xaml.cs", ""));
    }
}