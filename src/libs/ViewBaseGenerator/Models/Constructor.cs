namespace H.Generators;

internal readonly record struct Constructor(
    string Namespace,
    string Modifier,
    string Name,
    bool CreateReactiveUIWhenActivated,
    bool SetReactiveUIDataContext,
    string ViewModel,
    string BaseClass,
    Platform? Platform)
{
}
