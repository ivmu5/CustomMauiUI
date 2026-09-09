namespace MauiUiComponents;

/// <summary>
/// Содержит методы расширения для настройки горизонтального
/// и вертикального выравнивания текста в компонентах,
/// реализующих <see cref="ITextAlignmentComponent"/>.
/// </summary>
public static class TextAlignmentComponentExtensions
{
    #region Combined Alignment

    /// <summary>
    /// Центрирует текст одновременно по горизонтали и вертикали.
    /// </summary>
    /// <returns>
    /// Исходный компонент для продолжения fluent-цепочки.
    /// </returns>
    public static T TextCenter<T>(
        this T view)
        where T : ITextAlignmentComponent
    {
        return view
            .TextCenterHorizontal()
            .TextCenterVertical();
    }

    #endregion

    #region Horizontal Alignment

    /// <summary>
    /// Центрирует текст по горизонтали.
    /// </summary>
    public static T TextCenterHorizontal<T>(
        this T view)
        where T : ITextAlignmentComponent
    {
        view.HorizontalTextAlignment =
            TextAlignment.Center;

        return view;
    }

    /// <summary>
    /// Выравнивает текст по левому краю.
    /// </summary>
    public static T TextLeft<T>(
        this T view)
        where T : ITextAlignmentComponent
    {
        view.HorizontalTextAlignment =
            TextAlignment.Start;

        return view;
    }

    /// <summary>
    /// Выравнивает текст по правому краю.
    /// </summary>
    public static T TextRight<T>(
        this T view)
        where T : ITextAlignmentComponent
    {
        view.HorizontalTextAlignment =
            TextAlignment.End;

        return view;
    }

    /// <summary>
    /// Растягивает текст по доступной ширине
    /// с использованием выравнивания Justify.
    /// </summary>
    public static T TextFillHorizontal<T>(
        this T view)
        where T : ITextAlignmentComponent
    {
        view.HorizontalTextAlignment =
            TextAlignment.Justify;

        return view;
    }

    #endregion

    #region Vertical Alignment

    /// <summary>
    /// Центрирует текст по вертикали.
    /// </summary>
    public static T TextCenterVertical<T>(
        this T view)
        where T : ITextAlignmentComponent
    {
        view.VerticalTextAlignment =
            TextAlignment.Center;

        return view;
    }

    /// <summary>
    /// Выравнивает текст по верхнему краю.
    /// </summary>
    public static T TextTop<T>(
        this T view)
        where T : ITextAlignmentComponent
    {
        view.VerticalTextAlignment =
            TextAlignment.Start;

        return view;
    }

    /// <summary>
    /// Выравнивает текст по нижнему краю.
    /// </summary>
    public static T TextBottom<T>(
        this T view)
        where T : ITextAlignmentComponent
    {
        view.VerticalTextAlignment =
            TextAlignment.End;

        return view;
    }

    /// <summary>
    /// Растягивает текст по доступной высоте
    /// с использованием выравнивания Justify.
    /// </summary>
    public static T TextFillVertical<T>(
        this T view)
        where T : ITextAlignmentComponent
    {
        view.VerticalTextAlignment =
            TextAlignment.Justify;

        return view;
    }

    #endregion
}