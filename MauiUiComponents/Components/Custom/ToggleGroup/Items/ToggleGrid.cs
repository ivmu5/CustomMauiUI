namespace MauiUiComponents;

/// <summary>
/// Toggle-элемент на основе <see cref="Grid"/>,
/// позволяющий объединить несколько визуальных toggle-компонентов
/// в единый выбираемый элемент.
/// </summary>
/// <remarks>
/// Дочерние <see cref="IToggleItem"/> синхронизируют своё состояние
/// выбора с состоянием родительского <see cref="ToggleGrid"/>,
/// сохраняя при этом собственные действия и визуальную логику.
/// </remarks>
public class ToggleGrid : ToggleItem<Grid>
{
    #region Fields

    private int _childIndex;

    #endregion

    #region Children

    /// <summary>
    /// Добавляет дочерний toggle-элемент в указанную ячейку сетки
    /// и связывает его состояние выбора с состоянием родительского элемента.
    /// </summary>
    /// <param name="child">
    /// Дочерний toggle-элемент.
    /// </param>
    /// <param name="row">
    /// Индекс строки, в которую добавляется элемент.
    /// </param>
    /// <param name="column">
    /// Индекс колонки, в которую добавляется элемент.
    /// </param>
    /// <param name="rowSpan">
    /// Количество занимаемых строк.
    /// </param>
    /// <param name="columnSpan">
    /// Количество занимаемых колонок.
    /// </param>
    public void AddToggleChild(
        IToggleItem child,
        int row = 0,
        int column = 0,
        int rowSpan = 1,
        int columnSpan = 1)
    {
        ArgumentNullException.ThrowIfNull(child);

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

        /*
         * Нажатие должен обрабатывать весь ToggleGrid,
         * а не отдельный дочерний элемент.
         */
        child.View.InputTransparent =
            true;

        child.View.ViewFillHorizontal();

        View.AddChild(
            child.View,
            row,
            column,
            rowSpan,
            columnSpan);

        child.Initialize(
            IsSelected);

        AddSelectionSynchronization(
            child);
    }

    #endregion

    #region Selection Synchronization

    /// <summary>
    /// Добавляет действие, синхронизирующее состояние выбора
    /// дочернего toggle-элемента с состоянием родителя.
    /// </summary>
    /// <param name="child">
    /// Дочерний элемент, состояние которого необходимо синхронизировать.
    /// </param>
    private void AddSelectionSynchronization(
        IToggleItem child)
    {
        /*
         * Для каждого дочернего элемента создаётся уникальное имя,
         * чтобы действия синхронизации не конфликтовали
         * в словаре действий ToggleItem.
         */
        var actionName =
            $"SyncChildSelection:{_childIndex++}";

        AddAction(
            new ToggleAction<Grid>(
                View,
                actionName,
                _ => child.IsSelected = true,
                _ => child.IsSelected = false,
                ToggleActionTrigger.Initialization,
                ToggleActionTrigger.SelectionStateChanged));
    }

    #endregion
}