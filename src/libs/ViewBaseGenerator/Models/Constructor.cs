namespace H.Generators;

internal readonly record struct Constructor(
    string Modifier,
    string Name,
    bool CreateReactiveUIWhenActivated,
    bool SetReactiveUIDataContext)
{
    public static Constructor FromPath(
        string path,
        string modifier,
        bool createReactiveUIWhenActivated,
        bool setReactiveUIDataContext)
    {
        var viewName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(path));

        return new Constructor(modifier, viewName, createReactiveUIWhenActivated, setReactiveUIDataContext);
    }
}
