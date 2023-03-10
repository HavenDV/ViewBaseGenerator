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
            interfaces.Add(!constructor.BaseClass.Contains('.') && constructor.Framework != Framework.None
                ? GenerateTypeByPlatform(constructor.Framework, $"Controls.{constructor.BaseClass}")
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

    public static string GenerateTypeByPlatform(Framework framework, string name)
    {
        return (framework switch
        {
            Framework.Wpf => $"System.Windows.{name}",
            Framework.Uwp or Framework.Uno => $"Windows.UI.Xaml.{name}",
            Framework.WinUi or Framework.UnoWinUi => $"Microsoft.UI.Xaml.{name}",
            Framework.Avalonia => $"Avalonia.{name}",
            _ => throw new InvalidOperationException("Platform is not supported."),
        }).WithGlobalPrefix();
    }

    #endregion
}
