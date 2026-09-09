namespace MauiUiComponents;

/// <summary>
/// Определяет события жизненного цикла toggle-элемента,
/// при которых могут выполняться зарегистрированные действия.
/// </summary>
public enum ToggleActionTrigger
{
    /// <summary>
    /// Действие выполняется при первичной
    /// инициализации toggle-элемента.
    /// </summary>
    Initialization,

    /// <summary>
    /// Действие выполняется при изменении
    /// состояния выбора toggle-элемента.
    /// </summary>
    SelectionStateChanged
}