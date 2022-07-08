using H.Generators.Extensions;

namespace H.Generators;

internal static class ViewBaseCodeGenerator
{
    public static string GenerateViewBase(
        string @namespace,
        ViewBase viewBase)
    {
        return @$" 
#nullable enable

namespace {@namespace}
{{
    {viewBase.Modifier} abstract partial class {viewBase.Name}
    : {viewBase.BaseClass.WithGlobalPrefix()}<{viewBase.ViewModel.WithGlobalPrefix()}>
    {{
    }}
}}
 ".RemoveBlankLinesWhereOnlyWhitespaces();
    }
}