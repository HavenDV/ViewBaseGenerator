//HintName: TestView.Constructors.generated.cs
#nullable enable

namespace Views
{
    public partial class TestView : global::System.Windows.Controls.UserControl, global::ReactiveUI.IViewFor<global::ViewModels.TestViewModel>
    {
        /// <summary>
        /// The view model dependency property.
        /// </summary>
        public static readonly global::System.Windows.DependencyProperty ViewModelProperty =
            global::System.Windows.DependencyProperty.Register(
                "ViewModel",
                typeof(global::ViewModels.TestViewModel),
                typeof(TestView),
                new global::System.Windows.PropertyMetadata(default(global::ViewModels.TestViewModel)));
        /// <summary>
        /// Gets the binding root view model.
        /// </summary>
        public global::ViewModels.TestViewModel? BindingRoot => ViewModel;

        /// <inheritdoc/>
        public global::ViewModels.TestViewModel? ViewModel
        {
            get => (global::ViewModels.TestViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        /// <inheritdoc/>
        object? global::ReactiveUI.IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (global::ViewModels.TestViewModel?)value;
        }

        partial void BeforeInitializeComponent();
        partial void AfterInitializeComponent();

        public TestView()
        {
            BeforeInitializeComponent();

            InitializeComponent();

            AfterInitializeComponent();
        }
    }
}