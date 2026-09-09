using Microsoft.Maui.Layouts;

namespace MauiUiComponents;

/// <summary>
/// Содержит fluent-методы расширения для настройки
/// поведения и расположения элементов внутри <see cref="FlexLayout"/>.
/// </summary>
public static class FlexLayoutExtensions
{
    #region Grow

    /// <summary>
    /// Назначает всем дочерним элементам указанный коэффициент Grow,
    /// определяющий долю свободного пространства,
    /// которую каждый элемент может занять вдоль главной оси FlexLayout.
    /// </summary>
    /// <param name="layout">
    /// Контейнер, дочерние элементы которого необходимо настроить.
    /// </param>
    /// <param name="grow">
    /// Коэффициент Grow, применяемый ко всем дочерним элементам.
    /// По умолчанию равен 1.
    /// </param>
    /// <returns>
    /// Исходный <see cref="FlexLayout"/> для продолжения fluent-цепочки.
    /// </returns>
    public static T FlexEqualGrow<T>(
        this T layout,
        float grow = 1)
        where T : FlexLayout
    {
        ArgumentNullException.ThrowIfNull(layout);

        foreach (var child in layout.Children)
        {
            layout.SetGrow(
                child,
                grow);
        }

        return layout;
    }

    /// <summary>
    /// Добавляет элементы в контейнер и назначает каждому из них Grow = 1.
    /// Используется для равномерного распределения элементов
    /// по доступному пространству.
    /// </summary>
    public static T AddEqual<T>(
        this T layout,
        params View[] views)
        where T : FlexLayout
    {
        ArgumentNullException.ThrowIfNull(layout);
        ArgumentNullException.ThrowIfNull(views);

        foreach (var view in views)
        {
            ArgumentNullException.ThrowIfNull(view);

            FlexLayout.SetGrow(
                view,
                1);

            layout.Children.Add(
                view);
        }

        return layout;
    }

    /// <summary>
    /// Добавляет элемент в контейнер
    /// и устанавливает для него указанный коэффициент Grow.
    /// </summary>
    /// <param name="grow">
    /// Коэффициент распределения свободного пространства.
    /// Значение не может быть отрицательным.
    /// </param>
    public static T AddGrow<T>(
        this T layout,
        View view,
        int grow = 1)
        where T : FlexLayout
    {
        ArgumentNullException.ThrowIfNull(layout);
        ArgumentNullException.ThrowIfNull(view);

        if (grow < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(grow),
                "Grow не может быть отрицательным.");
        }

        FlexLayout.SetGrow(
            view,
            grow);

        layout.Children.Add(
            view);

        return layout;
    }

    #endregion

    #region Direction

    /// <summary>
    /// Устанавливает горизонтальное расположение дочерних элементов
    /// и отключает перенос на следующую строку.
    /// </summary>
    public static T FlexRow<T>(
        this T layout)
        where T : FlexLayout
    {
        ArgumentNullException.ThrowIfNull(layout);

        layout.Direction =
            FlexDirection.Row;

        layout.Wrap =
            FlexWrap.NoWrap;

        return layout;
    }

    /// <summary>
    /// Устанавливает вертикальное расположение дочерних элементов
    /// и отключает перенос на следующую колонку.
    /// </summary>
    public static T FlexColumn<T>(
        this T layout)
        where T : FlexLayout
    {
        ArgumentNullException.ThrowIfNull(layout);

        layout.Direction =
            FlexDirection.Column;

        layout.Wrap =
            FlexWrap.NoWrap;

        return layout;
    }

    #endregion

    #region Wrap

    /// <summary>
    /// Включает перенос дочерних элементов,
    /// если им не хватает места по основной оси.
    /// </summary>
    public static T FlexWrapContent<T>(
        this T layout)
        where T : FlexLayout
    {
        ArgumentNullException.ThrowIfNull(layout);

        layout.Wrap =
            FlexWrap.Wrap;

        return layout;
    }

    #endregion

    #region Justify Content

    /// <summary>
    /// Размещает элементы у начала основной оси.
    /// </summary>
    public static T FlexContentStart<T>(
        this T layout)
        where T : FlexLayout
    {
        return layout.SetJustifyContent(
            FlexJustify.Start);
    }

    /// <summary>
    /// Центрирует элементы по основной оси.
    /// </summary>
    public static T FlexContentCenter<T>(
        this T layout)
        where T : FlexLayout
    {
        return layout.SetJustifyContent(
            FlexJustify.Center);
    }

    /// <summary>
    /// Размещает элементы у конца основной оси.
    /// </summary>
    public static T FlexContentEnd<T>(
        this T layout)
        where T : FlexLayout
    {
        return layout.SetJustifyContent(
            FlexJustify.End);
    }

    /// <summary>
    /// Распределяет элементы с равными внешними отступами
    /// вокруг каждого элемента.
    /// </summary>
    public static T FlexContentSpaceAround<T>(
        this T layout)
        where T : FlexLayout
    {
        return layout.SetJustifyContent(
            FlexJustify.SpaceAround);
    }

    /// <summary>
    /// Распределяет элементы так, чтобы первый находился у начала,
    /// последний — у конца, а свободное место оставалось между ними.
    /// </summary>
    public static T FlexContentSpaceBetween<T>(
        this T layout)
        where T : FlexLayout
    {
        return layout.SetJustifyContent(
            FlexJustify.SpaceBetween);
    }

    /// <summary>
    /// Равномерно распределяет элементы по основной оси,
    /// включая одинаковое свободное пространство у краёв контейнера.
    /// </summary>
    public static T FlexContentSpaceEvenly<T>(
        this T layout)
        where T : FlexLayout
    {
        return layout.SetJustifyContent(
            FlexJustify.SpaceEvenly);
    }

    /// <summary>
    /// Устанавливает способ распределения дочерних элементов
    /// по основной оси <see cref="FlexLayout"/>.
    /// </summary>
    private static T SetJustifyContent<T>(
        this T layout,
        FlexJustify justify)
        where T : FlexLayout
    {
        ArgumentNullException.ThrowIfNull(layout);

        layout.JustifyContent =
            justify;

        return layout;
    }

    #endregion

    #region Align Items

    /// <summary>
    /// Выравнивает дочерние элементы
    /// по началу поперечной оси.
    /// </summary>
    public static T FlexAlignStart<T>(
        this T layout)
        where T : FlexLayout
    {
        return layout.SetAlignItems(
            FlexAlignItems.Start);
    }

    /// <summary>
    /// Центрирует дочерние элементы
    /// по поперечной оси.
    /// </summary>
    public static T FlexAlignCenter<T>(
        this T layout)
        where T : FlexLayout
    {
        return layout.SetAlignItems(
            FlexAlignItems.Center);
    }

    /// <summary>
    /// Выравнивает дочерние элементы
    /// по концу поперечной оси.
    /// </summary>
    public static T FlexAlignEnd<T>(
        this T layout)
        where T : FlexLayout
    {
        return layout.SetAlignItems(
            FlexAlignItems.End);
    }

    /// <summary>
    /// Растягивает дочерние элементы
    /// по доступному пространству поперечной оси.
    /// </summary>
    public static T FlexAlignStretch<T>(
        this T layout)
        where T : FlexLayout
    {
        return layout.SetAlignItems(
            FlexAlignItems.Stretch);
    }

    /// <summary>
    /// Устанавливает способ выравнивания дочерних элементов
    /// по поперечной оси <see cref="FlexLayout"/>.
    /// </summary>
    private static T SetAlignItems<T>(
        this T layout,
        FlexAlignItems alignItems)
        where T : FlexLayout
    {
        ArgumentNullException.ThrowIfNull(layout);

        layout.AlignItems =
            alignItems;

        return layout;
    }

    #endregion
}