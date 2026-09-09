namespace MauiUiComponents;

/// <summary>
/// Содержит методы расширения для массового добавления
/// дочерних элементов в контейнеры, наследуемые от <see cref="Layout"/>.
/// </summary>
public static class LayoutExtensions
{
    #region Children

    /// <summary>
    /// Добавляет переданные элементы в контейнер
    /// в том порядке, в котором они были указаны.
    /// </summary>
    /// <param name="layout">
    /// Контейнер, в который добавляются элементы.
    /// </param>
    /// <param name="views">
    /// Набор элементов для добавления.
    /// </param>
    /// <returns>
    /// Исходный контейнер для продолжения fluent-цепочки.
    /// </returns>
    public static TLayout AddChildren<TLayout>(
        this TLayout layout,
        params View[] views)
        where TLayout : Layout
    {
        ArgumentNullException.ThrowIfNull(layout);
        ArgumentNullException.ThrowIfNull(views);

        foreach (var view in views)
        {
            ArgumentNullException.ThrowIfNull(view);

            layout.Add(view);
        }

        return layout;
    }

    /// <summary>
    /// Добавляет последовательность элементов в контейнер
    /// в порядке их перечисления.
    /// </summary>
    /// <param name="layout">
    /// Контейнер, в который добавляются элементы.
    /// </param>
    /// <param name="views">
    /// Последовательность элементов для добавления.
    /// </param>
    /// <returns>
    /// Исходный контейнер для продолжения fluent-цепочки.
    /// </returns>
    public static TLayout AddChildren<TLayout>(
        this TLayout layout,
        IEnumerable<View> views)
        where TLayout : Layout
    {
        ArgumentNullException.ThrowIfNull(layout);
        ArgumentNullException.ThrowIfNull(views);

        foreach (var view in views)
        {
            ArgumentNullException.ThrowIfNull(view);

            layout.Add(view);
        }

        return layout;
    }

    #endregion
}