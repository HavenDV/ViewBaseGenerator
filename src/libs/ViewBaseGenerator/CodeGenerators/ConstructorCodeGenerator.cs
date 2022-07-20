using H.Generators.Extensions;

namespace H.Generators;

internal static class ConstructorCodeGenerator
{
    #region Methods

    public static string GenerateConstructor(Constructor constructor)
    {
        var setRx = constructor.SetReactiveUIDataContext || constructor.CreateReactiveUIWhenActivated;
        var interfaces = new List<string>();
        if (constructor.InheritFromViewBase)
        {
            interfaces.Add($"{constructor.Name}Base");
        }
        else if (!string.IsNullOrWhiteSpace(constructor.BaseClass))
        {
            interfaces.Add(!constructor.BaseClass.Contains('.') && constructor.Platform.HasValue
                ? GenerateTypeByPlatform(constructor.Platform.Value, $"Controls.{constructor.BaseClass}")
                : constructor.BaseClass.WithGlobalPrefix());
        }

        return @$"{(setRx ? @"
using ReactiveUI;
" : " ")}
 
#nullable enable

namespace {constructor.Namespace}
{{
    {constructor.Modifier} partial class {constructor.Name}{(interfaces.Any() ? $" : {string.Join(", ", interfaces)}" : "")}
    {{
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
