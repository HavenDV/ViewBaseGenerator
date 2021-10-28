﻿namespace H.Generators;

public readonly record struct Constructor(
    string Modifier,
    string Name,
    bool IsDeffered,
    bool SetReactiveUIDataContext)
{
    public static Constructor FromPath(
        string path,
        string modifier,
        bool isDeffered,
        bool setReactiveUIDataContext)
    {
        path = path ?? throw new ArgumentNullException(nameof(path));

        var viewName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(path));

        return new Constructor(modifier, viewName, isDeffered, setReactiveUIDataContext);
    }
}