namespace H.Generators;

internal readonly record struct Constructor(
    string Modifier,
    string Name,
    bool SetReactiveUIDataContext)
{
    public static Constructor FromPath(
        string path,
        string modifier,
        bool setReactiveUIDataContext)
    {
        var viewName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(path));

        return new Constructor(modifier, viewName, setReactiveUIDataContext);
    }
}
