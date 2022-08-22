//HintName: TestViewBase.ViewBase.generated.cs
#nullable enable

namespace Views
{
    public partial class TestViewBase
    : global::System.Windows.Controls.UserControl
    {
        /// <summary>
        /// The view model dependency property.
        /// </summary>
        public static readonly global::System.Windows.DependencyProperty ViewModelProperty =
            global::System.Windows.DependencyProperty.Register(
                "ViewModel",
                typeof(global::ViewModels.TestViewModel),
                typeof(TestViewBase),
                new global::System.Windows.PropertyMetadata(default(global::ViewModels.TestViewModel)));

        /// <summary>
        /// The view model property.
        /// </summary>
        public global::ViewModels.TestViewModel? ViewModel
        {
            get => (global::ViewModels.TestViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

    }
}