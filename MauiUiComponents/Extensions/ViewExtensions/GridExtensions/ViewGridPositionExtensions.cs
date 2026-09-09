namespace MauiUiComponents;

/// <summary>
/// Содержит методы расширения для настройки позиции
/// и размера элемента внутри <see cref="Grid"/>.
/// </summary>
public static class ViewGridPositionExtensions
{
    #region Position

    /// <summary>
    /// Устанавливает строку и колонку,
    /// в которых должен располагаться элемент.
    /// </summary>
    /// <param name="view">
    /// Элемент, положение которого изменяется.
    /// </param>
    /// <param name="row">
    /// Индекс строки.
    /// </param>
    /// <param name="column">
    /// Индекс колонки.
    /// </param>
    /// <returns>
    /// Исходный элемент для продолжения fluent-цепочки.
    /// </returns>
    public static T GridPosition<T>(
        this T view,
        int row,
        int column)
        where T : View
    {
        ArgumentNullException.ThrowIfNull(view);

        if (row < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(row),
                "Индекс строки не может быть отрицательным.");
        }

        if (column < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(column),
                "Индекс колонки не может быть отрицательным.");
        }

        Grid.SetRow(
            view,
            row);

        Grid.SetColumn(
            view,
            column);

        return view;
    }

    /// <summary>
    /// Устанавливает положение элемента внутри сетки
    /// и количество занимаемых им строк и колонок.
    /// </summary>
    /// <param name="view">
    /// Элемент, положение которого изменяется.
    /// </param>
    /// <param name="row">
    /// Индекс строки.
    /// </param>
    /// <param name="column">
    /// Индекс колонки.
    /// </param>
    /// <param name="rowSpan">
    /// Количество занимаемых строк.
    /// </param>
    /// <param name="columnSpan">
    /// Количество занимаемых колонок.
    /// </param>
    /// <returns>
    /// Исходный элемент для продолжения fluent-цепочки.
    /// </returns>
    public static T GridPosition<T>(
        this T view,
        int row,
        int column,
        int rowSpan = 1,
        int columnSpan = 1)
        where T : View
    {
        return view
            .GridPosition(
                row,
                column)
            .GridRowSpan(
                rowSpan)
            .GridColumnSpan(
                columnSpan);
    }

    #endregion

    #region Span

    /// <summary>
    /// Устанавливает количество колонок,
    /// занимаемых элементом внутри <see cref="Grid"/>.
    /// </summary>
    public static T GridColumnSpan<T>(
        this T view,
        int span)
        where T : View
    {
        ArgumentNullException.ThrowIfNull(view);

        if (span < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(span),
                "Количество занимаемых колонок должно быть не меньше 1.");
        }

        Grid.SetColumnSpan(
            view,
            span);

        return view;
    }

    /// <summary>
    /// Устанавливает количество строк,
    /// занимаемых элементом внутри <see cref="Grid"/>.
    /// </summary>
    public static T GridRowSpan<T>(
        this T view,
        int span)
        where T : View
    {
        ArgumentNullException.ThrowIfNull(view);

        if (span < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(span),
                "Количество занимаемых строк должно быть не меньше 1.");
        }

        Grid.SetRowSpan(
            view,
            span);

        return view;
    }

    #endregion
}