using MauiUiSettings;

namespace MauiUiComponents;

public static class BorderExtensions
{
    public static BaseBorder<T> WithBorder<T>(
        this T view,
        ComponentStore componentStore,
        ColorVariant strokeColor = ColorVariant.Primary,
        ColorVariant backgroundColor = ColorVariant.None)
        where T : View
    {
        return componentStore.Base.Border(view, strokeColor, backgroundColor);
    }

    public static T BorderRoundRectangleBind<T>(
        this T view,
        UiServiceStore uiService,
        BindableProperty? targetProperty = null)
        where T : Border
    {
        return view.Bind(
            x => x.StrokeShape,
            uiService.CornerRadiusService,
            x => x.RoundRectangle);
    }
}
