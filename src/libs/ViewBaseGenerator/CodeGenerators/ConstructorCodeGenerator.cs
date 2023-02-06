using H.Generators.Extensions;

namespace H.Generators;

internal static class ConstructorCodeGenerator
{
    #region Methods

    public static string GenerateConstructor(Constructor constructor)
    {
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

        return @$" 
 
#nullable enable

namespace {constructor.Namespace}
{{
    {constructor.Modifier} partial class {constructor.Name}{(interfaces.Any() ? $" : {string.Join(", ", interfaces)}" : "")}
    {{
        partial void BeforeInitializeComponent();
        partial void AfterInitializeComponent();

        public {constructor.Name}()
        {{
            BeforeInitializeComponent();

            InitializeComponent();

            AfterInitializeComponent();
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
