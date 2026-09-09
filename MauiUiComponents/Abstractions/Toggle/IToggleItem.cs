namespace MauiUiComponents;

/// <summary>
/// Определяет общий контракт элемента,
/// участвующего в работе <see cref="ToggleGroup{TItem, TLayout}"/>.
/// </summary>
public interface IToggleItem
{
    /// <summary>
    /// Получает визуальное представление toggle-элемента.
    /// </summary>
    View View { get; }

    /// <summary>
    /// Получает или устанавливает текущее состояние выбора элемента.
    /// </summary>
    bool IsSelected { get; set; }

    /// <summary>
    /// Выполняет первичную инициализацию элемента
    /// с указанным состоянием выбора.
    /// </summary>
    /// <param name="isSelected">
    /// Начальное состояние выбора элемента.
    /// </param>
    void Initialize(
        bool isSelected);

    /// <summary>
    /// Добавляет одно или несколько действий,
    /// связанных с состоянием toggle-элемента.
    /// </summary>
    /// <param name="actions">
    /// Действия, которые необходимо зарегистрировать.
    /// </param>
    void AddAction(
        params IToggleAction[] actions);

    /// <summary>
    /// Выполняет зарегистрированные действия,
    /// соответствующие указанным типам событий.
    /// </summary>
    /// <param name="triggers">
    /// Типы событий, по которым необходимо обновить действия.
    /// Если список пуст, реализация может выполнить все действия.
    /// </param>
    void UpdateActions(
        params ToggleActionTrigger[] triggers);
}