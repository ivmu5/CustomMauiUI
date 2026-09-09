using System.Diagnostics;

namespace MauiUiComponents;

/// <summary>
/// Представляет действие, связанное с конкретным <see cref="View"/>,
/// которое выполняется в зависимости от состояния выбора toggle-элемента.
/// </summary>
/// <typeparam name="TView">
/// Тип представления, для которого выполняется действие.
/// </typeparam>
[DebuggerDisplay("{ActionName,nq}")]
public sealed class ToggleAction<TView> : IToggleAction
    where TView : View
{
    #region Fields

    private readonly TView _view;

    #endregion

    #region Properties

    /// <summary>
    /// Получает уникальное имя действия внутри toggle-элемента.
    /// </summary>
    public string ActionName { get; }

    /// <summary>
    /// Получает набор событий, при которых
    /// должно выполняться данное действие.
    /// </summary>
    public IReadOnlySet<ToggleActionTrigger> TriggerTypes { get; }

    /// <summary>
    /// Получает действие, выполняемое,
    /// когда toggle-элемент находится в выбранном состоянии.
    /// </summary>
    public Action<TView> OnSelected { get; }

    /// <summary>
    /// Получает действие, выполняемое,
    /// когда toggle-элемент находится в невыбранном состоянии.
    /// </summary>
    public Action<TView>? OnUnselected { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Создаёт toggle-действие с отдельным поведением
    /// для выбранного и невыбранного состояния.
    /// </summary>
    /// <param name="view">
    /// Представление, над которым выполняется действие.
    /// </param>
    /// <param name="actionName">
    /// Уникальное имя действия.
    /// </param>
    /// <param name="onSelected">
    /// Действие для выбранного состояния.
    /// </param>
    /// <param name="onUnselected">
    /// Действие для невыбранного состояния.
    /// Может отсутствовать.
    /// </param>
    /// <param name="triggers">
    /// События жизненного цикла toggle-элемента,
    /// при которых должно выполняться действие.
    /// </param>
    public ToggleAction(
        TView view,
        string actionName,
        Action<TView> onSelected,
        Action<TView>? onUnselected,
        params ToggleActionTrigger[] triggers)
    {
        ArgumentNullException.ThrowIfNull(view);
        ArgumentException.ThrowIfNullOrWhiteSpace(actionName);
        ArgumentNullException.ThrowIfNull(onSelected);
        ArgumentNullException.ThrowIfNull(triggers);

        _view =
            view;

        ActionName =
            actionName;

        OnSelected =
            onSelected;

        OnUnselected =
            onUnselected;

        TriggerTypes =
            new HashSet<ToggleActionTrigger>(
                triggers);
    }

    /// <summary>
    /// Создаёт toggle-действие, которое имеет поведение
    /// только для выбранного состояния.
    /// </summary>
    /// <param name="view">
    /// Представление, над которым выполняется действие.
    /// </param>
    /// <param name="actionName">
    /// Уникальное имя действия.
    /// </param>
    /// <param name="onSelected">
    /// Действие для выбранного состояния.
    /// </param>
    /// <param name="triggers">
    /// События жизненного цикла toggle-элемента,
    /// при которых должно выполняться действие.
    /// </param>
    public ToggleAction(
        TView view,
        string actionName,
        Action<TView> onSelected,
        params ToggleActionTrigger[] triggers)
        : this(
            view,
            actionName,
            onSelected,
            null,
            triggers)
    {
    }

    #endregion

    #region Trigger Check

    /// <summary>
    /// Проверяет, связано ли действие
    /// с указанным типом события.
    /// </summary>
    public bool HasTrigger(
        ToggleActionTrigger trigger)
    {
        return TriggerTypes.Contains(
            trigger);
    }

    /// <summary>
    /// Проверяет, связано ли действие хотя бы
    /// с одним из указанных типов событий.
    /// </summary>
    public bool HasTrigger(
        params ToggleActionTrigger[] triggers)
    {
        ArgumentNullException.ThrowIfNull(triggers);

        return triggers.Any(
            TriggerTypes.Contains);
    }

    #endregion

    #region Execution

    /// <summary>
    /// Выполняет действие, соответствующее
    /// текущему состоянию выбора toggle-элемента.
    /// </summary>
    /// <param name="isSelected">
    /// <see langword="true"/>, если элемент выбран;
    /// иначе <see langword="false"/>.
    /// </param>
    public void Execute(
        bool isSelected)
    {
        if (isSelected)
        {
            OnSelected(
                _view);

            return;
        }

        OnUnselected?.Invoke(
            _view);
    }

    #endregion
}