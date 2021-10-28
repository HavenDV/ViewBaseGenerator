namespace H.Generators;

public static class ConstructorCodeGenerator
{
    #region Methods

    public static string GenerateConstructors(
        string @namespace,
        IReadOnlyCollection<Constructor> classes)
    {
        return @$"
namespace {@namespace}
{{
{
string.Join(Environment.NewLine, classes.Select(GenerateConstructor))
}
}}
";
    }

    public static string GenerateConstructor(
        Constructor constructor)
    {
        var (modifier, name) = constructor;

        return @$"
    {modifier} partial class {name}
    {{
        partial void BeforeInitializeComponent();
        partial void AfterInitializeComponent();

        public FileView()
        {{
            BeforeInitializeComponent();

            InitializeComponent();

            AfterInitializeComponent();
        }}

    }}
";
    }

    #endregion
}
