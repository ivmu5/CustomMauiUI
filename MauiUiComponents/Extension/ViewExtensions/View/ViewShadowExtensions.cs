namespace MauiUiComponents;

public static class ViewShadowExtensions
{
    public static T ViewAddShadow<T>(
        this T view,
        Color? color = null,
        float radius = 15f,
        float opacity = 0.5f,
        float offsetX = 3f,
        float offsetY = 3f)
        where T : View
    {
        color ??= Colors.Black;

        view.Shadow = new Shadow
        {
            Brush = color,
            Radius = radius,
            Opacity = opacity,
            Offset = new Point(offsetX, offsetY)
        };

        return view;
    }
}
