namespace MauiUiComponents;

/// <summary>
/// Содержит методы расширения для настройки внутренних отступов
/// элементов, наследуемых от <see cref="Layout"/>.
/// </summary>
public static class ViewPaddingExtensions
{
    #region Padding

    /// <summary>
    /// Устанавливает внутренние отступы контейнера
    /// с использованием готового значения <see cref="Thickness"/>.
    /// </summary>
    /// <param name="layout">
    /// Контейнер, для которого изменяются внутренние отступы.
    /// </param>
    /// <param name="padding">
    /// Значение внутренних отступов.
    /// </param>
    /// <returns>
    /// Исходный контейнер для продолжения fluent-цепочки.
    /// </returns>
    public static T LayoutPadding<T>(
        this T layout,
        Thickness padding)
        where T : Layout
    {
        ArgumentNullException.ThrowIfNull(layout);

        layout.Padding =
            padding;

        return layout;
    }

    /// <summary>
    /// Устанавливает одинаковый внутренний отступ
    /// со всех сторон контейнера.
    /// </summary>
    /// <param name="uniform">
    /// Размер отступа для всех сторон.
    /// </param>
    public static T LayoutPadding<T>(
        this T layout,
        double uniform)
        where T : Layout
    {
        return layout.LayoutPadding(
            new Thickness(uniform));
    }

    /// <summary>
    /// Устанавливает одинаковые горизонтальные
    /// и одинаковые вертикальные внутренние отступы.
    /// </summary>
    /// <param name="horizontal">
    /// Внутренний отступ слева и справа.
    /// </param>
    /// <param name="vertical">
    /// Внутренний отступ сверху и снизу.
    /// </param>
    public static T LayoutPadding<T>(
        this T layout,
        double horizontal,
        double vertical)
        where T : Layout
    {
        return layout.LayoutPadding(
            new Thickness(
                horizontal,
                vertical));
    }

    /// <summary>
    /// Устанавливает индивидуальные внутренние отступы
    /// для каждой стороны контейнера.
    /// </summary>
    /// <param name="left">
    /// Внутренний отступ слева.
    /// </param>
    /// <param name="top">
    /// Внутренний отступ сверху.
    /// </param>
    /// <param name="right">
    /// Внутренний отступ справа.
    /// </param>
    /// <param name="bottom">
    /// Внутренний отступ снизу.
    /// </param>
    public static T LayoutPadding<T>(
        this T layout,
        double left,
        double top,
        double right,
        double bottom)
        where T : Layout
    {
        return layout.LayoutPadding(
            new Thickness(
                left,
                top,
                right,
                bottom));
    }

    #endregion
}