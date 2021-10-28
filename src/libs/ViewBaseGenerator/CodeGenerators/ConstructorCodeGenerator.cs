namespace H.Generators;

public static class ConstructorCodeGenerator
{
    #region Methods

    public static string GenerateConstructors(
        string @namespace,
        IReadOnlyCollection<Constructor> constructors)
    {
        var usingReactiveUI = constructors.Any(x => x.SetReactiveUIDataContext);

        return @$"{(usingReactiveUI ? @"
using ReactiveUI;" : string.Empty)}

namespace {@namespace}
{{
{
string.Join(Environment.NewLine, constructors.Select(GenerateConstructor))
}
}}
";
    }

    public static string GenerateConstructor(
        Constructor constructor)
    {
        var (modifier, name, setReactiveUIDataContext) = constructor;

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
{(setReactiveUIDataContext ? @"
            _ = this.WhenActivated(disposable =>
            {
                DataContext = ViewModel;
            });" : string.Empty)}
        }}

    }}
";
    }

    #endregion
}
