namespace H.Generators;

internal static class ViewBaseCodeGenerator
{
    #region Methods

    public static string GenerateViewBases(
        string @namespace,
        IReadOnlyCollection<ViewBase> classes)
    {
        return @$"
namespace {@namespace}
{{
{
string.Join("\n", classes.Select(GenerateViewBase))
}
}}
";
    }

    public static string GenerateViewBase(
        ViewBase @class)
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
