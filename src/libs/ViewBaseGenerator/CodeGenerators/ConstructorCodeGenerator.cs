namespace H.Generators;

internal static class ConstructorCodeGenerator
{
    #region Methods

    public static string GenerateConstructor(
        string @namespace,
        Constructor constructor)
    {
        var (modifier, name, setRx) = constructor;
        var usingReactiveUi = constructor.SetReactiveUiDataContext;

        return @$"{(usingReactiveUi ? @"
using ReactiveUI;
using System.Reactive.Disposables;" : string.Empty)}

#nullable enable

namespace {@namespace}
{{
    {modifier} partial class {name}
    {{
        partial void BeforeInitializeComponent();
        partial void AfterInitializeComponent();

{(setRx ? @"
        partial void AfterWhenActivated(global::System.Reactive.Disposables.CompositeDisposable disposables);" : string.Empty)}

        public {name}()
        {{
            BeforeInitializeComponent();

            InitializeComponent();

            AfterInitializeComponent();
{(setRx ? @"
            _ = this.WhenActivated(disposables =>
            {
                DataContext = ViewModel;

                if (ViewModel == null)
                {
                    return;
                }

                AfterWhenActivated(disposables);
            });" : string.Empty)}
        }}
    }}
}}
";
    }

    #endregion
}
