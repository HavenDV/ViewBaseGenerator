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
        var (modifier, name, deffer, setRx) = constructor;

        return @$"
    {modifier} partial class {name}
    {{
        partial void BeforeInitializeComponent();
        partial void AfterInitializeComponent();

{(setRx ? @"
        partial void AfterWhenActivated(CompositeDisposable disposables);" : string.Empty)}

{(deffer ? @"
        partial void InitializeComponentDeffered();" : string.Empty)}
{(deffer && setRx ? @"
        partial void WhenActivatedDeffered();" : string.Empty)}

        public {name}()
        {{
            BeforeInitializeComponent();
{(deffer ? @"
            InitializeComponentDeffered();" : @"
            InitializeComponent();")}

            AfterInitializeComponent();
{(setRx && !deffer ? @"
            _ = this.WhenActivated(disposables =>
            {
                DataContext = ViewModel;

                if (ViewModel != null)
                {
                    return;
                }

                AfterWhenActivated(disposables);
            });" :
setRx && deffer ? @"
            WhenActivatedDeffered();" : string.Empty)}
        }}

    }}
";
    }

    #endregion
}
