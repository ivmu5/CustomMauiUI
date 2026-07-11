using MauiUiSettings;
using System.Linq.Expressions;

namespace MauiUiComponents;

public class ToggleGroupStyleStore
{
    private readonly UiServiceStore _uiServices;

    public ToggleGroupStyleStore(UiServiceStore uiServices)
    {
        _uiServices = uiServices;
    }

    public ToggleBehavior<TView> ToggleColor<TView>(
        View view,
        Expression<Func<TView, object?>> propertyExpression,
        ColorVariant selectedColor = ColorVariant.Primary,
        ColorVariant unselectedColor = ColorVariant.Secondary)
        where TView : View, new()
    {
        return new(
            view,
            (view) => view.ColorBind(_uiServices, propertyExpression, selectedColor),
            (view) => view.ColorBind(_uiServices, propertyExpression, unselectedColor),
            ToggleTrigger.Initialization,
            ToggleTrigger.UIStateChange);
    }

    public ToggleBehavior<TView> ToggleBackgroundColor<TView>(
        View view,
        ColorVariant selectedColor = ColorVariant.Primary,
        ColorVariant unselectedColor = ColorVariant.Secondary)
        where TView : View, new()
    {
        return ToggleColor<TView>(
            view,
            x => x.Background,
            selectedColor,
            unselectedColor);
    }
}
