namespace MauiUiComponents;

public static class ViewMarginExtensions
{
    public static T ViewMargin<T>(
        this T view,
        Thickness margin)
        where T : View
    {
        view.Margin = margin;
        return view;
    }

    public static T ViewMargin<T>(
        this T view,
        double uniform)
        where T : View
    {
        return view
            .ViewMargin(new Thickness(uniform));
    }

    public static T ViewMargin<T>(
        this T view,
        double horizontal,
        double vertical)
        where T : View
    {
        return view
            .ViewMargin(new Thickness(horizontal, vertical));
    }

    public static T ViewMargin<T>(
        this T view,
        double left,
        double top,
        double right,
        double bottom)
        where T : View
    {
        return view
            .ViewMargin(new Thickness(left, top, right, bottom));
    }
}
