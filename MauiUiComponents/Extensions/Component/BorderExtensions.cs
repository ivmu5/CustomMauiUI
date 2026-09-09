using MauiUiSettings;

namespace MauiUiComponents;

/// <summary>
/// Содержит методы расширения для создания и настройки
/// компонентов <see cref="Border"/>.
/// </summary>
public static class BorderExtensions
{
    #region Creation

    /// <summary>
    /// Оборачивает указанный элемент в стандартный <see cref="BaseBorder{TView}"/>
    /// и применяет к нему настройки цвета и скругления из <see cref="ComponentStore"/>.
    /// </summary>
    /// <typeparam name="TView">
    /// Тип вложенного представления.
    /// </typeparam>
    /// <param name="view">
    /// Представление, которое будет помещено внутрь рамки.
    /// </param>
    /// <param name="componentStore">
    /// Хранилище компонентов и UI-сервисов.
    /// </param>
    /// <param name="strokeColor">
    /// Цвет обводки рамки.
    /// </param>
    /// <param name="backgroundColor">
    /// Цвет фона рамки.
    /// </param>
    /// <returns>
    /// Созданная рамка с вложенным представлением.
    /// </returns>
    public static BaseBorder<TView> WithBorder<TView>(
        this TView view,
        ComponentStore componentStore,
        ColorVariant strokeColor = ColorVariant.Primary,
        ColorVariant backgroundColor = ColorVariant.None)
        where TView : View
    {
        ArgumentNullException.ThrowIfNull(view);
        ArgumentNullException.ThrowIfNull(componentStore);

        return componentStore.Base.Border(
            view,
            strokeColor,
            backgroundColor);
    }

    #endregion

    #region Corner Radius

    /// <summary>
    /// Привязывает форму рамки к текущему значению скругления
    /// из <see cref="CornerRadiusService"/>.
    /// </summary>
    /// <typeparam name="TBorder">
    /// Тип рамки.
    /// </typeparam>
    /// <param name="border">
    /// Рамка, для которой настраивается форма.
    /// </param>
    /// <param name="uiServices">
    /// Набор UI-сервисов, содержащий <see cref="CornerRadiusService"/>.
    /// </param>
    /// <returns>
    /// Исходная рамка для продолжения fluent-цепочки.
    /// </returns>
    public static TBorder BorderRoundRectangleBind<TBorder>(
        this TBorder border,
        UiServiceStore uiServices)
        where TBorder : Border
    {
        ArgumentNullException.ThrowIfNull(border);
        ArgumentNullException.ThrowIfNull(uiServices);

        return border.Bind(
            target => target.StrokeShape,
            uiServices.CornerRadiusService,
            service => service.RoundRectangle);
    }

    #endregion
}