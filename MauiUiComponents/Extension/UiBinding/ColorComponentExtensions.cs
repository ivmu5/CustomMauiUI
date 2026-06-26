using MauiUiSettings;
using System.Linq.Expressions;

namespace MauiUiComponents;

public static class ColorComponentExtensions
{
    public static Expression<Func<ColorService, object>> GetColorPropertyExpression(this ColorVariant variant)
    {
        return variant switch
        {
            ColorVariant.Primary =>
                cs => cs.Primary,

            ColorVariant.Secondary =>
                cs => cs.Secondary,

            ColorVariant.Tertiary =>
                cs => cs.Tertiary,

            ColorVariant.Text =>
                cs => cs.Text,

            ColorVariant.Background =>
                cs => cs.Background,

            ColorVariant.Blur =>
                cs => cs.Blur,

            _ => throw new ArgumentOutOfRangeException(nameof(variant))
        };
    }

    public static T ColorBind<T>(
        this T view,
        UiServiceStore uiServices,
        Expression<Func<T, object?>> propertyExpression,
        ColorVariant variant)
        where T : BindableObject
    {
        if (variant == ColorVariant.None)
        {
            var targetProperty = propertyExpression?.GetBindableProperty();

            if (targetProperty is null)
                return view;

            view.RemoveBinding(targetProperty);
            view.SetValue(targetProperty, Colors.Transparent);
            return view;
        }

        return view.Bind(
            propertyExpression,
            uiServices.ColorService,
            GetColorPropertyExpression(variant));
    }

    public static T ColorBackgroundBind<T>(
        this T view,
        UiServiceStore uiServices,
        ColorVariant variant = ColorVariant.Background)
        where T : View
    {
        return view.ColorBind(
            uiServices,
            x => x.BackgroundColor,
            variant);
    }

    public static T TextColorBind<T>(
        this T view,
        UiServiceStore uiServices,
        ColorVariant variant = ColorVariant.Text)
        where T : BindableObject, ITextComponent
    {
        return view.ColorBind(
            uiServices,
            x => x.TextColor,
            variant);
    }
}