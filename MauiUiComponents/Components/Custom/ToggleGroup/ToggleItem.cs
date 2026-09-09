namespace MauiUiComponents;

/// <summary>
/// Базовая реализация toggle-элемента,
/// связывающая визуальное представление <typeparamref name="TView"/>
/// с состоянием выбора и набором действий.
/// </summary>
/// <typeparam name="TView">
/// Тип визуального представления toggle-элемента.
/// </typeparam>
public class ToggleItem<TView> :
    BindableObject,
    IToggleItem
    where TView : View, new()
{
    #region Fields

    private readonly Dictionary<string, IToggleAction>
        _actions = new();

    private bool _initialized;

    #endregion

    #region Properties

    /// <summary>
    /// Получает визуальное представление toggle-элемента.
    /// </summary>
    public TView View { get; }

    /// <summary>
    /// Получает визуальное представление элемента
    /// через контракт <see cref="IToggleItem"/>.
    /// </summary>
    View IToggleItem.View =>
        View;

    /// <summary>
    /// Получает зарегистрированные действия toggle-элемента.
    /// </summary>
    public IReadOnlyCollection<IToggleAction> Actions =>
        _actions.Values;

    /// <summary>
    /// Получает или устанавливает текущее состояние выбора элемента.
    /// </summary>
    public bool IsSelected
    {
        get =>
            (bool)GetValue(IsSelectedProperty);

        set =>
            SetValue(
                IsSelectedProperty,
                value);
    }

    /// <summary>
    /// Bindable-свойство для <see cref="IsSelected"/>.
    /// </summary>
    public static readonly BindableProperty IsSelectedProperty =
        BindableProperty.Create(
            nameof(IsSelected),
            typeof(bool),
            typeof(ToggleItem<TView>),
            false,
            propertyChanged: OnIsSelectedChanged);

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт toggle-элемент с указанным представлением
    /// либо создаёт новый экземпляр <typeparamref name="TView"/>.
    /// </summary>
    /// <param name="view">
    /// Представление toggle-элемента.
    /// Если не указано, создаётся новый экземпляр <typeparamref name="TView"/>.
    /// </param>
    public ToggleItem(
        TView? view = null)
    {
        View =
            view ?? new TView();
    }

    #endregion

    #region Initialization

    /// <summary>
    /// Выполняет первичную инициализацию toggle-элемента
    /// и применяет действия, связанные с событием
    /// <see cref="ToggleActionTrigger.Initialization"/>.
    /// </summary>
    /// <param name="isSelected">
    /// Начальное состояние выбора элемента.
    /// </param>
    public void Initialize(
        bool isSelected)
    {
        if (_initialized)
            return;

        IsSelected =
            isSelected;

        _initialized =
            true;

        UpdateActions(
            ToggleActionTrigger.Initialization);
    }

    #endregion

    #region Actions

    /// <summary>
    /// Добавляет одно или несколько действий,
    /// связанных с toggle-элементом.
    /// </summary>
    /// <remarks>
    /// Действия идентифицируются по <see cref="IToggleAction.ActionName"/>.
    /// Повторное добавление действия с тем же именем игнорируется.
    /// </remarks>
    /// <param name="actions">
    /// Действия, которые необходимо зарегистрировать.
    /// </param>
    public void AddAction(
        params IToggleAction[] actions)
    {
        ArgumentNullException.ThrowIfNull(actions);

        foreach (var action in actions)
        {
            ArgumentNullException.ThrowIfNull(action);

            _actions.TryAdd(
                action.ActionName,
                action);
        }
    }

    /// <summary>
    /// Выполняет зарегистрированные действия,
    /// соответствующие указанным типам событий.
    /// </summary>
    /// <param name="triggers">
    /// Типы событий, для которых необходимо выполнить действия.
    /// Если список пуст, выполняются все зарегистрированные действия.
    /// </param>
    public void UpdateActions(
        params ToggleActionTrigger[] triggers)
    {
        ArgumentNullException.ThrowIfNull(triggers);

        IEnumerable<IToggleAction> actions =
            _actions.Values;

        if (triggers.Length > 0)
        {
            actions =
                actions.Where(
                    action => action.HasTrigger(triggers));
        }

        foreach (var action in actions)
        {
            action.Execute(
                IsSelected);
        }
    }

    #endregion

    #region Selection State

    /// <summary>
    /// Обрабатывает изменение bindable-свойства
    /// <see cref="IsSelected"/>.
    /// </summary>
    private static void OnIsSelectedChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        if (Equals(
                oldValue,
                newValue))
        {
            return;
        }

        var item =
            (ToggleItem<TView>)bindable;

        item.OnSelectionStateChanged();
    }

    /// <summary>
    /// Выполняет действия, связанные
    /// с изменением состояния выбора элемента.
    /// </summary>
    private void OnSelectionStateChanged()
    {
        /*
         * До завершения Initialize() состояние можно установить,
         * но действия выполнять ещё нельзя: элемент пока
         * не считается полностью инициализированным.
         */
        if (!_initialized)
            return;

        UpdateActions(
            ToggleActionTrigger.SelectionStateChanged);
    }

    #endregion
}