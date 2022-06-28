using H.Generators.Extensions;

namespace H.Generators;

internal static class ConstructorCodeGenerator
{
    #region Methods

    public static string GenerateConstructor(
        string @namespace,
        Constructor constructor)
    {
        var (modifier, name, setDataContext, createWhenActivated) = constructor;
        var setRx = setDataContext || createWhenActivated;

        return @$"{(setRx ? @"
using ReactiveUI;
" : " ")}
 
#nullable enable

namespace {@namespace}
{{
    {modifier} partial class {name}
    {{
        partial void BeforeInitializeComponent();
        partial void AfterInitializeComponent();
{(setRx ? @"
        partial void AfterWhenActivated(global::System.Reactive.Disposables.CompositeDisposable disposables);" : " ")}

        public {name}()
        {{
            BeforeInitializeComponent();

            InitializeComponent();

            AfterInitializeComponent();
{(setRx ? @$"
            _ = this.WhenActivated(disposables =>
            {{
{(setDataContext ? @" 
                DataContext = ViewModel;
" : " ")}
                AfterWhenActivated(disposables);
            }});" : " ")}
        }}
    }}
}}
 ".RemoveBlankLinesWhereOnlyWhitespaces();
    }

    #endregion
}
