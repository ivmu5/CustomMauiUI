namespace MauiUiComponents;

/// <summary>
/// Содержит fluent-методы расширения для настройки
/// строк, колонок и дочерних элементов <see cref="Grid"/>.
/// </summary>
public static class GridExtensions
{
    #region Rows

    /// <summary>
    /// Добавляет строку с указанной высотой.
    /// </summary>
    /// <param name="grid">
    /// Сетка, в которую добавляется строка.
    /// </param>
    /// <param name="height">
    /// Высота новой строки.
    /// </param>
    /// <returns>
    /// Исходная сетка для продолжения fluent-цепочки.
    /// </returns>
    public static T AddRow<T>(
        this T grid,
        GridLength height)
        where T : Grid
    {
        ArgumentNullException.ThrowIfNull(grid);

        grid.RowDefinitions.Add(
            new RowDefinition(height));

        return grid;
    }

    /// <summary>
    /// Добавляет строку с фиксированной высотой.
    /// </summary>
    public static T AddRow<T>(
        this T grid,
        double height)
        where T : Grid
    {
        return grid.AddRow(
            new GridLength(height));
    }

    /// <summary>
    /// Добавляет строку, занимающую оставшееся доступное пространство.
    /// </summary>
    public static T AddStarRow<T>(
        this T grid)
        where T : Grid
    {
        return grid.AddRow(
            GridLength.Star);
    }

    /// <summary>
    /// Добавляет строку, размер которой определяется её содержимым.
    /// </summary>
    public static T AddAutoRow<T>(
        this T grid)
        where T : Grid
    {
        return grid.AddRow(
            GridLength.Auto);
    }

    #endregion

    #region Columns

    /// <summary>
    /// Добавляет колонку с указанной шириной.
    /// </summary>
    /// <param name="grid">
    /// Сетка, в которую добавляется колонка.
    /// </param>
    /// <param name="width">
    /// Ширина новой колонки.
    /// </param>
    /// <returns>
    /// Исходная сетка для продолжения fluent-цепочки.
    /// </returns>
    public static T AddColumn<T>(
        this T grid,
        GridLength width)
        where T : Grid
    {
        ArgumentNullException.ThrowIfNull(grid);

        grid.ColumnDefinitions.Add(
            new ColumnDefinition(width));

        return grid;
    }

    /// <summary>
    /// Добавляет колонку с фиксированной шириной.
    /// </summary>
    public static T AddColumn<T>(
        this T grid,
        double width)
        where T : Grid
    {
        return grid.AddColumn(
            new GridLength(width));
    }

    /// <summary>
    /// Добавляет колонку, занимающую оставшееся доступное пространство.
    /// </summary>
    public static T AddStarColumn<T>(
        this T grid)
        where T : Grid
    {
        return grid.AddColumn(
            GridLength.Star);
    }

    /// <summary>
    /// Добавляет колонку, размер которой определяется её содержимым.
    /// </summary>
    public static T AddAutoColumn<T>(
        this T grid)
        where T : Grid
    {
        return grid.AddColumn(
            GridLength.Auto);
    }

    #endregion

    #region Children

    /// <summary>
    /// Добавляет дочерний элемент в сетку
    /// без изменения его текущей позиции.
    /// </summary>
    /// <param name="grid">
    /// Сетка, в которую добавляется элемент.
    /// </param>
    /// <param name="view">
    /// Добавляемый элемент.
    /// </param>
    /// <returns>
    /// Исходная сетка для продолжения fluent-цепочки.
    /// </returns>
    public static T AddChild<T>(
        this T grid,
        View view)
        where T : Grid
    {
        ArgumentNullException.ThrowIfNull(grid);
        ArgumentNullException.ThrowIfNull(view);

        grid.Add(view);

        return grid;
    }

    /// <summary>
    /// Добавляет дочерний элемент в указанную ячейку сетки
    /// и при необходимости растягивает его на несколько строк или колонок.
    /// </summary>
    /// <param name="grid">
    /// Сетка, в которую добавляется элемент.
    /// </param>
    /// <param name="view">
    /// Добавляемый элемент.
    /// </param>
    /// <param name="row">
    /// Индекс строки.
    /// </param>
    /// <param name="column">
    /// Индекс колонки.
    /// </param>
    /// <param name="rowSpan">
    /// Количество занимаемых строк. Минимальное значение — 1.
    /// </param>
    /// <param name="columnSpan">
    /// Количество занимаемых колонок. Минимальное значение — 1.
    /// </param>
    /// <returns>
    /// Исходная сетка для продолжения fluent-цепочки.
    /// </returns>
    public static T AddChild<T>(
        this T grid,
        View view,
        int row,
        int column,
        int rowSpan = 1,
        int columnSpan = 1)
        where T : Grid
    {
        ArgumentNullException.ThrowIfNull(grid);
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

        if (rowSpan < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(rowSpan),
                "Количество занимаемых строк должно быть не меньше 1.");
        }

        if (columnSpan < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(columnSpan),
                "Количество занимаемых колонок должно быть не меньше 1.");
        }

        view.GridPosition(
            row,
            column,
            rowSpan,
            columnSpan);

        grid.Add(view);

        return grid;
    }

    #endregion
}