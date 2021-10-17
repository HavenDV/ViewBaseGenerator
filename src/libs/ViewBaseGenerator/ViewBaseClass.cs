namespace H.Generators;

public readonly record struct ViewBaseClass(
    string Modifier,
    string Name,
    string BaseClass,
    string ViewModel)
{
    public static ViewBaseClass FromPath(
        string path,
        string modifier,
        string baseClass,
        string viewModelNamespace)
    {
        path = path ?? throw new ArgumentNullException(nameof(path));

        var viewName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(path));
        var viewBaseName = viewName.Replace("View", "ViewBase");
        var viewModelName = $"{viewModelNamespace}.{viewName.Replace("View", "ViewModel")}";

        return new ViewBaseClass(modifier, viewBaseName, baseClass, viewModelName);
    }
}
