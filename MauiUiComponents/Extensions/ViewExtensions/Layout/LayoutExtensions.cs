namespace MauiUiComponents;

public static class LayoutExtensions
{
    public static TLayout AddChildren<TLayout>(
        this TLayout layout,
        params View[] views)
        where TLayout : Layout
    {
        foreach (var view in views)
            layout.Add(view);

        return layout;
    }

    public static TLayout AddChildren<TLayout>(
        this TLayout layout,
        IEnumerable<View> views)
        where TLayout : Layout
    {
        foreach (var view in views)
            layout.Add(view);

        return layout;
    }
}
