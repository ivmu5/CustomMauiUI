using MauiUiSettings;
using System.Linq.Expressions;

namespace MauiUiComponents;

public class ToggleGroupStyleStore
{
    private readonly ComponentStore _componentStore;

    public ToggleGroupStyleStore(ComponentStore componentStore)
    {
        _componentStore = componentStore;
    }

    public ToggleAction<TView> ToggleColor<TView>(
        TView view,
        Expression<Func<TView, object?>> propertyExpression,
        ColorVariant selectedColor = ColorVariant.Primary,
        ColorVariant unselectedColor = ColorVariant.Secondary)
        where TView : View
    {
        return new(
            view,
            "SetColor" + propertyExpression.GetPropertyName(),
            (view) => view.ColorBind(_componentStore.UiServices, propertyExpression, selectedColor),
            (view) => view.ColorBind(_componentStore.UiServices, propertyExpression, unselectedColor),
            ToggleActionTrigger.Initialization,
            ToggleActionTrigger.UIStateChange);
    }

    public ToggleAction<TView> ToggleBackgroundColor<TView>(
        TView view,
        ColorVariant selectedColor = ColorVariant.Primary,
        ColorVariant unselectedColor = ColorVariant.Secondary)
        where TView : View
    {
        return ToggleColor(
            view,
            x => x.Background,
            selectedColor,
            unselectedColor);
    }
}
