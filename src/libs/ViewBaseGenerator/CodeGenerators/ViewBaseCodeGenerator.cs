using H.Generators.Extensions;

namespace H.Generators;

internal static class ViewBaseCodeGenerator
{
    public static string GenerateViewBase(
        string @namespace,
        ViewBase @class)
    {
        var (modifier, name, @base, viewModel) = @class;

        return @$"
#nullable enable

namespace {@namespace}
{{
    {modifier} abstract partial class {name}
    : {@base}<{viewModel.WithGlobalPrefix()}>
    {{
    }}
}}
";
    }
}