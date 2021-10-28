namespace H.Generators.UnitTests;

[TestClass]
public class CodeGeneratorTests
{
    [TestMethod]
    public void GenerateViewBases()
    {
        var code = ViewBaseCodeGenerator.GenerateViewBases("Test", new[]
        {
                new ViewBase("public", "TestViewBase", "ReactiveControl", "TestViewModel"),
            });

        code.Should().Be(@"
namespace Test
{

    public abstract partial class TestViewBase
    : ReactiveControl<TestViewModel>
    {
    }

}
");
    }
}
