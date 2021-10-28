namespace H.Generators;

public static class DefferedConstructorCodeGenerator
{
    #region Methods

    public static string GenerateConstructors(
        string @namespace,
        IReadOnlyCollection<Constructor> constructors)
    {
        var usingReactiveUI = constructors.Any(x => x.SetReactiveUIDataContext);

        return @$"{(usingReactiveUI ? @"
using ReactiveUI;
using System.Reactive.Disposables;" : string.Empty)}

#nullable enable

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
        var (modifier, name, _, setRx) = constructor;

        return @$"
    {modifier} partial class {name}
    {{
        public void InitializeComponentDeffered()
        {{
             InitializeComponent();
        }}

{(setRx ? @"
        public void WhenActivatedDeffered()
        {
            _ = this.WhenActivated(disposables =>
            {
                DataContext = ViewModel;

                if (ViewModel != null)
                {
                    return;
                }

                AfterWhenActivated(disposables);
            });
        }" : string.Empty)}
    }}
";
    }

    #endregion
}
