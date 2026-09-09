namespace MauiUiComponents;

/// <summary>
/// Определяет действие, выполняемое для элемента переключателя
/// в зависимости от его состояния выбора.
/// </summary>
public interface IToggleAction
{
    /// <summary>
    /// Получает уникальное имя действия внутри конкретного toggle-элемента.
    /// </summary>
    string ActionName { get; }

    /// <summary>
    /// Проверяет, должно ли действие выполняться
    /// для указанного типа события.
    /// </summary>
    /// <param name="trigger">
    /// Тип события, для которого выполняется проверка.
    /// </param>
    /// <returns>
    /// <see langword="true"/>, если действие связано
    /// с указанным событием; иначе <see langword="false"/>.
    /// </returns>
    bool HasTrigger(
        ToggleActionTrigger trigger);

    /// <summary>
    /// Проверяет, связано ли действие хотя бы
    /// с одним из указанных типов событий.
    /// </summary>
    /// <param name="triggers">
    /// Типы событий для проверки.
    /// </param>
    /// <returns>
    /// <see langword="true"/>, если найден хотя бы один
    /// зарегистрированный тип события; иначе <see langword="false"/>.
    /// </returns>
    bool HasTrigger(
        params ToggleActionTrigger[] triggers);

    /// <summary>
    /// Выполняет действие в соответствии
    /// с текущим состоянием выбора элемента.
    /// </summary>
    /// <param name="isSelected">
    /// <see langword="true"/>, если элемент выбран;
    /// иначе <see langword="false"/>.
    /// </param>
    void Execute(
        bool isSelected);
}