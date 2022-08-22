using H.Generators.Extensions;

namespace H.Generators;

internal static class ViewBaseCodeGenerator
{
    public static string GenerateViewBase(ViewBase viewBase)
    {
        var baseClass = !viewBase.BaseClass.Contains('.') && viewBase.Platform.HasValue
            ? ConstructorCodeGenerator.GenerateTypeByPlatform(viewBase.Platform.Value, $"Controls.{viewBase.BaseClass}")
            : $"{viewBase.BaseClass.WithGlobalPrefix()}<{viewBase.ViewModel.WithGlobalPrefix()}>";

        var viewModelType = viewBase.ViewModel.WithGlobalPrefix();
        var dependencyProperty = viewBase.Platform.HasValue
            ? ConstructorCodeGenerator.GenerateTypeByPlatform(viewBase.Platform.Value, "DependencyProperty")
            : string.Empty;

        return @$" 
#nullable enable

namespace {viewBase.Namespace}
{{
    {viewBase.Modifier} abstract partial class {viewBase.Name}
    : {baseClass}
    {{
{(viewBase.AddViewModelDependencyProperty ? @$" 
        /// <summary>
        /// The view model dependency property.
        /// </summary>
        public static readonly {dependencyProperty} ViewModelProperty =
            {dependencyProperty}.Register(
                ""ViewModel"",
                typeof({viewModelType}),
                typeof({viewBase.Name}),
                new global::System.Windows.PropertyMetadata(default({viewModelType})));

        /// <summary>
        /// The view model property.
        /// </summary>
        public {viewModelType}? ViewModel
        {{
            get => ({viewModelType})GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }}
" : " ")}
    }}
}}
 ".RemoveBlankLinesWhereOnlyWhitespaces();
    }
}