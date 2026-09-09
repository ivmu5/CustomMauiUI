namespace MauiUiComponents;

/// <summary>
/// Содержит методы расширения для настройки внешних отступов
/// элементов <see cref="View"/>.
/// </summary>
public static class ViewMarginExtensions
{
    #region Margin

    /// <summary>
    /// Устанавливает внешние отступы элемента
    /// с использованием готового значения <see cref="Thickness"/>.
    /// </summary>
    /// <param name="view">
    /// Элемент, для которого изменяются внешние отступы.
    /// </param>
    /// <param name="margin">
    /// Значение внешних отступов.
    /// </param>
    /// <returns>
    /// Исходный элемент для продолжения fluent-цепочки.
    /// </returns>
    public static T ViewMargin<T>(
        this T view,
        Thickness margin)
        where T : View
    {
        ArgumentNullException.ThrowIfNull(view);

        view.Margin =
            margin;

        return view;
    }

    /// <summary>
    /// Устанавливает одинаковый внешний отступ
    /// со всех сторон элемента.
    /// </summary>
    /// <param name="uniform">
    /// Размер отступа для всех сторон.
    /// </param>
    public static T ViewMargin<T>(
        this T view,
        double uniform)
        where T : View
    {
        return view.ViewMargin(
            new Thickness(uniform));
    }

    /// <summary>
    /// Устанавливает одинаковые горизонтальные
    /// и одинаковые вертикальные внешние отступы.
    /// </summary>
    /// <param name="horizontal">
    /// Отступ слева и справа.
    /// </param>
    /// <param name="vertical">
    /// Отступ сверху и снизу.
    /// </param>
    public static T ViewMargin<T>(
        this T view,
        double horizontal,
        double vertical)
        where T : View
    {
        return view.ViewMargin(
            new Thickness(
                horizontal,
                vertical));
    }

    /// <summary>
    /// Устанавливает индивидуальные внешние отступы
    /// для каждой стороны элемента.
    /// </summary>
    /// <param name="left">
    /// Отступ слева.
    /// </param>
    /// <param name="top">
    /// Отступ сверху.
    /// </param>
    /// <param name="right">
    /// Отступ справа.
    /// </param>
    /// <param name="bottom">
    /// Отступ снизу.
    /// </param>
    public static T ViewMargin<T>(
        this T view,
        double left,
        double top,
        double right,
        double bottom)
        where T : View
    {
        return view.ViewMargin(
            new Thickness(
                left,
                top,
                right,
                bottom));
    }

    #endregion
}