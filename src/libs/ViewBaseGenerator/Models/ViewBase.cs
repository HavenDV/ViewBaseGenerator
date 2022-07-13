namespace H.Generators;

internal readonly record struct ViewBase(
    string Namespace,
    string Modifier,
    string Name,
    string BaseClass,
    string ViewModel,
    Platform? Platform)
{
}
