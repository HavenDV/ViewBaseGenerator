namespace H.Generators;

internal readonly record struct Constructor(
    string Namespace,
    string Modifier,
    string Name,
    bool InheritFromViewBase,
    bool CreateReactiveUIWhenActivated,
    bool SetReactiveUIDataContext,
    string BaseClass,
    Platform? Platform)
{
}
