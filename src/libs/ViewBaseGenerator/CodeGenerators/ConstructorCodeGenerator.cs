using H.Generators.Extensions;

namespace H.Generators;

internal static class ConstructorCodeGenerator
{
    #region Methods

    public static string GenerateConstructor(
        string @namespace,
        Constructor constructor)
    {
        var setRx = constructor.SetReactiveUIDataContext || constructor.CreateReactiveUIWhenActivated;
        var generateViewModelProperty = !constructor.ViewModel.StartsWith(".");
        var viewModelType = constructor.ViewModel.WithGlobalPrefix();
        var interfaces = generateViewModelProperty
            ? $", global::ReactiveUI.IViewFor<{viewModelType}>"
            : string.Empty;
        var baseClass = !string.IsNullOrWhiteSpace(constructor.BaseClass)
            ? $" : {constructor.BaseClass.WithGlobalPrefix()}"
            : string.Empty;
        var dependencyProperty = constructor.Platform.HasValue
            ? GenerateTypeByPlatform(constructor.Platform.Value, "DependencyProperty")
            : string.Empty;
        var propertyMetadata = constructor.Platform.HasValue
            ? GenerateTypeByPlatform(constructor.Platform.Value, "PropertyMetadata")
            : string.Empty;

        return @$"{(setRx ? @"
using ReactiveUI;
" : " ")}
 
#nullable enable

namespace {@namespace}
{{
    {constructor.Modifier} partial class {constructor.Name}{baseClass}{interfaces}
    {{
{(generateViewModelProperty ? @$" 
        /// <summary>
        /// The view model dependency property.
        /// </summary>
        public static readonly {dependencyProperty} ViewModelProperty =
            {dependencyProperty}.Register(
                ""ViewModel"",
                typeof({viewModelType}),
                typeof({constructor.Name}),
                new {propertyMetadata}(default({viewModelType})));
				
        /// <summary>
        /// Gets the binding root view model.
        /// </summary>
        public {viewModelType}? BindingRoot => ViewModel;

        /// <inheritdoc/>
        public {viewModelType}? ViewModel
        {{
            get => ({viewModelType})GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }}

        /// <inheritdoc/>
        object? global::ReactiveUI.IViewFor.ViewModel
        {{
            get => ViewModel;
            set => ViewModel = ({viewModelType}?)value;
        }}
" : " ")}
 
        partial void BeforeInitializeComponent();
        partial void AfterInitializeComponent();
{(setRx ? @"
        partial void AfterWhenActivated(global::System.Reactive.Disposables.CompositeDisposable disposables);" : " ")}

        public {constructor.Name}()
        {{
            BeforeInitializeComponent();

            InitializeComponent();

            AfterInitializeComponent();
{(setRx ? @$"
            _ = this.WhenActivated(disposables =>
            {{
{(constructor.SetReactiveUIDataContext ? @" 
                DataContext = ViewModel;
" : " ")}
                AfterWhenActivated(disposables);
            }});" : " ")}
        }}
    }}
}}
 ".RemoveBlankLinesWhereOnlyWhitespaces();
    }

    public static string GenerateTypeByPlatform(Platform platform, string name)
    {
        return (platform switch
        {
            Platform.WPF => $"System.Windows.{name}",
            Platform.UWP or Platform.Uno => $"Windows.UI.Xaml.{name}",
            Platform.WinUI or Platform.UnoWinUI => $"Microsoft.UI.Xaml.{name}",
            Platform.Avalonia => $"Avalonia.{name}",
            _ => throw new InvalidOperationException("Platform is not supported."),
        }).WithGlobalPrefix();
    }

    #endregion
}
