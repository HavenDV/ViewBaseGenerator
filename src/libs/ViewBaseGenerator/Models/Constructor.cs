namespace H.Generators;

internal readonly record struct Constructor(
    string Modifier,
    string Name,
    bool SetReactiveUiDataContext)
{
    public static Constructor FromPath(
        string path,
        string modifier,
        bool setReactiveUiDataContext)
    {
        var viewName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(path));

        return new Constructor(modifier, viewName, setReactiveUiDataContext);
    }
}
