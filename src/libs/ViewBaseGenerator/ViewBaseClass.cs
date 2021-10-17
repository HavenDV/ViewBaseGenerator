namespace H.Generators;

public readonly record struct ViewBaseClass(
    string Modifier,
    string Name,
    string BaseClass,
    string ViewModel)
{
    public static ViewBaseClass FromPath(
        string path,
        string baseClass,
        string modifier)
    {
        path = path ?? throw new ArgumentNullException(nameof(path));

        var viewName = Path.GetFileNameWithoutExtension(path);
        var viewBaseName = viewName.Replace("View", "ViewBase");
        var viewModelName = viewName.Replace("View", "ViewModel");

        return new ViewBaseClass(modifier, viewBaseName, baseClass, viewModelName);
    }
}
