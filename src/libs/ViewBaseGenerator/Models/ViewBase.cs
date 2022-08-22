namespace H.Generators;

internal readonly record struct ViewBase(
    string Namespace,
    string Modifier,
    string Name,
    string BaseClass,
    string ViewModel,
    bool AddViewModelDependencyProperty,
    Platform? Platform)
{
}
