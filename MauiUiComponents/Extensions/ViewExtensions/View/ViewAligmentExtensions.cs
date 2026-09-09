namespace MauiUiComponents;

/// <summary>
/// Содержит методы расширения для настройки
/// горизонтального и вертикального выравнивания элементов <see cref="View"/>.
/// </summary>
public static class ViewAlignmentExtensions
{
    #region Combined Alignment

    /// <summary>
    /// Устанавливает одновременно горизонтальное
    /// и вертикальное выравнивание элемента.
    /// </summary>
    /// <param name="view">
    /// Элемент, для которого изменяется выравнивание.
    /// </param>
    /// <param name="horizontal">
    /// Горизонтальный вариант размещения.
    /// </param>
    /// <param name="vertical">
    /// Вертикальный вариант размещения.
    /// </param>
    /// <returns>
    /// Исходный элемент для продолжения fluent-цепочки.
    /// </returns>
    public static T ViewAlignment<T>(
        this T view,
        LayoutOptions horizontal,
        LayoutOptions vertical)
        where T : View
    {
        ArgumentNullException.ThrowIfNull(view);

        return view
            .ViewHorizontalOptions(horizontal)
            .ViewVerticalOptions(vertical);
    }

    #endregion

    #region Horizontal Options

    /// <summary>
    /// Устанавливает горизонтальный вариант размещения элемента.
    /// </summary>
    public static T ViewHorizontalOptions<T>(
        this T view,
        LayoutOptions horizontal)
        where T : View
    {
        ArgumentNullException.ThrowIfNull(view);

        view.HorizontalOptions =
            horizontal;

        return view;
    }

    /// <summary>
    /// Центрирует элемент по горизонтали.
    /// </summary>
    public static T ViewHorizontalCenter<T>(
        this T view)
        where T : View
    {
        return view.ViewHorizontalOptions(
            LayoutOptions.Center);
    }

    /// <summary>
    /// Растягивает элемент по доступной ширине.
    /// </summary>
    public static T ViewFillHorizontal<T>(
        this T view)
        where T : View
    {
        return view.ViewHorizontalOptions(
            LayoutOptions.Fill);
    }

    /// <summary>
    /// Размещает элемент у начала горизонтальной оси.
    /// </summary>
    public static T ViewHorizontalStart<T>(
        this T view)
        where T : View
    {
        return view.ViewHorizontalOptions(
            LayoutOptions.Start);
    }

    /// <summary>
    /// Размещает элемент у конца горизонтальной оси.
    /// </summary>
    public static T ViewHorizontalEnd<T>(
        this T view)
        where T : View
    {
        return view.ViewHorizontalOptions(
            LayoutOptions.End);
    }

    #endregion

    #region Vertical Options

    /// <summary>
    /// Устанавливает вертикальный вариант размещения элемента.
    /// </summary>
    public static T ViewVerticalOptions<T>(
        this T view,
        LayoutOptions vertical)
        where T : View
    {
        ArgumentNullException.ThrowIfNull(view);

        view.VerticalOptions =
            vertical;

        return view;
    }

    /// <summary>
    /// Центрирует элемент по вертикали.
    /// </summary>
    public static T ViewVerticalCenter<T>(
        this T view)
        where T : View
    {
        return view.ViewVerticalOptions(
            LayoutOptions.Center);
    }

    /// <summary>
    /// Растягивает элемент по доступной высоте.
    /// </summary>
    public static T ViewFillVertical<T>(
        this T view)
        where T : View
    {
        return view.ViewVerticalOptions(
            LayoutOptions.Fill);
    }

    /// <summary>
    /// Размещает элемент у начала вертикальной оси.
    /// </summary>
    public static T ViewVerticalStart<T>(
        this T view)
        where T : View
    {
        return view.ViewVerticalOptions(
            LayoutOptions.Start);
    }

    /// <summary>
    /// Размещает элемент у конца вертикальной оси.
    /// </summary>
    public static T ViewVerticalEnd<T>(
        this T view)
        where T : View
    {
        return view.ViewVerticalOptions(
            LayoutOptions.End);
    }

    #endregion

    #region Common Alignment

    /// <summary>
    /// Центрирует элемент одновременно
    /// по горизонтали и вертикали.
    /// </summary>
    public static T ViewCenter<T>(
        this T view)
        where T : View
    {
        return view.ViewAlignment(
            LayoutOptions.Center,
            LayoutOptions.Center);
    }

    /// <summary>
    /// Растягивает элемент одновременно
    /// по горизонтали и вертикали.
    /// </summary>
    public static T ViewFillBoth<T>(
        this T view)
        where T : View
    {
        return view.ViewAlignment(
            LayoutOptions.Fill,
            LayoutOptions.Fill);
    }

    #endregion

    #region Corner Alignment

    /// <summary>
    /// Размещает элемент в левом верхнем углу.
    /// </summary>
    public static T ViewTopLeft<T>(
        this T view)
        where T : View
    {
        return view.ViewAlignment(
            LayoutOptions.Start,
            LayoutOptions.Start);
    }

    /// <summary>
    /// Размещает элемент в правом верхнем углу.
    /// </summary>
    public static T ViewTopRight<T>(
        this T view)
        where T : View
    {
        return view.ViewAlignment(
            LayoutOptions.End,
            LayoutOptions.Start);
    }

    /// <summary>
    /// Размещает элемент в левом нижнем углу.
    /// </summary>
    public static T ViewBottomLeft<T>(
        this T view)
        where T : View
    {
        return view.ViewAlignment(
            LayoutOptions.Start,
            LayoutOptions.End);
    }

    /// <summary>
    /// Размещает элемент в правом нижнем углу.
    /// </summary>
    public static T ViewBottomRight<T>(
        this T view)
        where T : View
    {
        return view.ViewAlignment(
            LayoutOptions.End,
            LayoutOptions.End);
    }

    #endregion
}