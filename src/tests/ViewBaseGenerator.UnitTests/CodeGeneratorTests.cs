namespace H.Generators.UnitTests;

[TestClass]
public class CodeGeneratorTests
{
    [TestMethod]
    public void GenerateTest()
    {
        var code = CodeGenerator.GenerateViewBaseClasses("Test", new[]
        {
                new ViewBaseClass("public", "TestViewBase", "ReactiveControl", "TestViewModel"),
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
