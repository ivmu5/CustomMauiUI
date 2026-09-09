using MauiUiSettings;
using System.Linq.Expressions;

namespace MauiUiComponents;

/// <summary>
/// Содержит методы расширения для привязки цветовых свойств
/// UI-компонентов к глобальному <see cref="ColorService"/>.
/// </summary>
public static class ColorComponentExtensions
{
    #region Color Expression

    /// <summary>
    /// Возвращает выражение доступа к свойству
    /// <see cref="ColorService"/>, соответствующему указанному
    /// цветовому варианту.
    /// </summary>
    /// <param name="variant">
    /// Цветовой вариант, для которого требуется получить свойство.
    /// </param>
    /// <returns>
    /// Выражение доступа к соответствующему цвету
    /// в <see cref="ColorService"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Указан <see cref="ColorVariant.None"/>, который
    /// не соответствует свойству <see cref="ColorService"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Передано неизвестное значение <see cref="ColorVariant"/>.
    /// </exception>
    public static Expression<Func<ColorService, object?>>
        GetColorPropertyExpression(
            this ColorVariant variant)
    {
        return variant switch
        {
            ColorVariant.Primary =>
                service => service.Primary,

            ColorVariant.Secondary =>
                service => service.Secondary,

            ColorVariant.Tertiary =>
                service => service.Tertiary,

            ColorVariant.Text =>
                service => service.Text,

            ColorVariant.Background =>
                service => service.Background,

            ColorVariant.Blur =>
                service => service.Blur,

            ColorVariant.None =>
                throw new InvalidOperationException(
                    $"{nameof(ColorVariant.None)} не связан " +
                    $"с цветовым свойством {nameof(ColorService)}."),

            _ =>
                throw new ArgumentOutOfRangeException(
                    nameof(variant),
                    variant,
                    "Неизвестный цветовой вариант.")
        };
    }

    #endregion

    #region Color Binding

    /// <summary>
    /// Привязывает указанное цветовое свойство компонента
    /// к соответствующему цвету из <see cref="ColorService"/>.
    /// </summary>
    /// <typeparam name="T">
    /// Тип настраиваемого объекта.
    /// </typeparam>
    /// <param name="view">
    /// Объект, цветовое свойство которого необходимо настроить.
    /// </param>
    /// <param name="uiServices">
    /// Набор глобальных UI-сервисов.
    /// </param>
    /// <param name="propertyExpression">
    /// Выражение, указывающее целевое цветовое свойство.
    /// </param>
    /// <param name="variant">
    /// Цветовой вариант, который необходимо применить.
    /// </param>
    /// <returns>
    /// Исходный объект для продолжения fluent-цепочки.
    /// </returns>
    public static T ColorBind<T>(
        this T view,
        UiServiceStore uiServices,
        Expression<Func<T, object?>> propertyExpression,
        ColorVariant variant)
        where T : BindableObject
    {
        ArgumentNullException.ThrowIfNull(view);
        ArgumentNullException.ThrowIfNull(uiServices);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        if (variant == ColorVariant.None)
        {
            var targetProperty =
                propertyExpression.GetBindableProperty();

            /*
             * None означает отсутствие цветовой привязки.
             * Сначала удаляем возможный предыдущий Binding,
             * а затем явно делаем свойство прозрачным.
             */
            view.RemoveBinding(
                targetProperty);

            view.SetValue(
                targetProperty,
                Colors.Transparent);

            return view;
        }

        return view.Bind(
            propertyExpression,
            uiServices.ColorService,
            variant.GetColorPropertyExpression());
    }

    #endregion

    #region Background

    /// <summary>
    /// Привязывает цвет фона визуального элемента
    /// к глобальному <see cref="ColorService"/>.
    /// </summary>
    /// <remarks>
    /// Метод работает с <see cref="VisualElement"/>,
    /// поэтому может использоваться как с обычными View,
    /// так и со страницами <see cref="Page"/>.
    /// </remarks>
    /// <typeparam name="T">
    /// Тип визуального элемента.
    /// </typeparam>
    /// <param name="target">
    /// Визуальный элемент, фон которого необходимо настроить.
    /// </param>
    /// <param name="uiServices">
    /// Набор глобальных UI-сервисов.
    /// </param>
    /// <param name="variant">
    /// Цветовой вариант фона.
    /// </param>
    /// <returns>
    /// Исходный элемент для продолжения fluent-цепочки.
    /// </returns>
    public static T ColorBackgroundBind<T>(
        this T target,
        UiServiceStore uiServices,
        ColorVariant variant = ColorVariant.Background)
        where T : VisualElement
    {
        ArgumentNullException.ThrowIfNull(
            target);

        ArgumentNullException.ThrowIfNull(
            uiServices);

        return target.ColorBind(
            uiServices,
            element => element.BackgroundColor,
            variant);
    }

    #endregion

    #region Text

    /// <summary>
    /// Привязывает цвет текста компонента
    /// к глобальному <see cref="ColorService"/>.
    /// </summary>
    /// <param name="view">
    /// Текстовый компонент, цвет которого необходимо настроить.
    /// </param>
    /// <param name="uiServices">
    /// Набор глобальных UI-сервисов.
    /// </param>
    /// <param name="variant">
    /// Цветовой вариант текста.
    /// </param>
    /// <returns>
    /// Исходный компонент для продолжения fluent-цепочки.
    /// </returns>
    public static T TextColorBind<T>(
        this T view,
        UiServiceStore uiServices,
        ColorVariant variant = ColorVariant.Text)
        where T : BindableObject, ITextComponent
    {
        return view.ColorBind(
            uiServices,
            target => target.TextColor,
            variant);
    }

    #endregion
}