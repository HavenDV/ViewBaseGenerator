namespace H.Generators;

internal readonly record struct Constructor(
    string Namespace,
    string Modifier,
    string Name,
    bool InheritFromViewBase,
    string BaseClass,
    Framework Framework);
