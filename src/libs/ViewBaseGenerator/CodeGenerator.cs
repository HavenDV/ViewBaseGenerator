namespace H.Generators;

public static class CodeGenerator
{
    #region Methods

    public static string GenerateViewBaseClasses(
        string @namespace,
        IReadOnlyCollection<ViewBaseClass> classes)
    {
        return @$"
namespace {@namespace}
{{
{
string.Join(Environment.NewLine, classes.Select(GenerateViewBaseClass))
}
}}
";
    }

    public static string GenerateViewBaseClass(
        ViewBaseClass @class)
    {
        var (modifier, name, @base, viewModel) = @class;

        return @$"
    {modifier} abstract partial class {name}
    : {@base}<{viewModel}>
    {{
    }}
";
    }

    #endregion
}
