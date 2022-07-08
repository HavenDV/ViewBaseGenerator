using H.Generators.Extensions;

namespace H.Generators;

internal static class ViewBaseCodeGenerator
{
    public static string GenerateViewBase(
        string @namespace,
        ViewBase viewBase)
    {
        var baseClass = !viewBase.BaseClass.Contains('.') && viewBase.Platform.HasValue
            ? ConstructorCodeGenerator.GenerateTypeByPlatform(viewBase.Platform.Value, $"Controls.{viewBase.BaseClass}")
            : viewBase.BaseClass.WithGlobalPrefix();

        return @$" 
#nullable enable

namespace {@namespace}
{{
    {viewBase.Modifier} abstract partial class {viewBase.Name}
    : {baseClass}<{viewBase.ViewModel.WithGlobalPrefix()}>
    {{
    }}
}}
 ".RemoveBlankLinesWhereOnlyWhitespaces();
    }
}