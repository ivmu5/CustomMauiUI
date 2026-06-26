namespace MauiUiComponents;

public static class ViewPaddingExtensions
{
    public static T LayoutPadding<T>(
        this T view,
        Thickness padding)
        where T : Layout
    {
        view.Padding = padding;
        return view;
    }

    public static T LayoutPadding<T>(
        this T view,
        double uniform)
        where T : Layout
    {
        return view
            .LayoutPadding(new Thickness(uniform));
    }

    public static T LayoutPadding<T>(
        this T view,
        double horizontal,
        double vertical)
        where T : Layout
    {
        return view
            .LayoutPadding(new Thickness(horizontal, vertical));
    }

    public static T LayoutPadding<T>(
        this T view,
        double left,
        double top,
        double right,
        double bottom)
        where T : Layout
    {
        return view
            .LayoutPadding(new Thickness(left, top, right, bottom));
    }
}
