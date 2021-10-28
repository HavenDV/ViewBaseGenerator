namespace H.Generators;

public static class ViewBaseCodeGenerator
{
    #region Methods

    public static string GenerateViewBases(
        string @namespace,
        IReadOnlyCollection<ViewBaseClass> classes)
    {
        return @$"
namespace {@namespace}
{{
{
string.Join(Environment.NewLine, classes.Select(GenerateViewBase))
}
}}
";
    }

    public static string GenerateViewBase(
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
