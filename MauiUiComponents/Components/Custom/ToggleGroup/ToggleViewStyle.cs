using MauiUiSettings;
using System.Linq.Expressions;

namespace MauiUiComponents;

public class ToggleViewStyle<TView>
    where TView : View
{
    public Expression<Func<TView, object?>> PropertyExpression { get; init; }
    public ColorVariant SelectedColor { get; init; }
    public ColorVariant UnselectedColor { get; init; }

    public ToggleViewStyle(
        Expression<Func<TView, object?>> propertyExpression,
        ColorVariant selectedColor,
        ColorVariant unselectedColor)
    {
        PropertyExpression = propertyExpression;
        SelectedColor = selectedColor;
        UnselectedColor = unselectedColor;
    }
}
