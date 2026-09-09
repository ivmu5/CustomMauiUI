using SQLiteStorage;

namespace MauiUiComponents;

/// <summary>
/// Представляет группу toggle-элементов с единственным выбранным значением.
/// </summary>
/// <typeparam name="TItem">
/// Тип значения, связанного с каждым toggle-элементом.
/// </typeparam>
/// <typeparam name="TLayout">
/// Тип контейнера, используемого для размещения toggle-элементов.
/// </typeparam>
public class ToggleGroup<TItem, TLayout> : ContentView
    where TLayout : Layout, new()
    where TItem : notnull
{
    #region Fields

    private readonly ComponentStore _componentStore;

    private readonly Dictionary<TItem, IToggleItem>
        _views = new();

    private readonly Grid _rootGrid;

    private Func<TItem, IToggleItem> _itemTemplate;

    private bool _useCaptionLabel;

    #endregion

    #region Properties

    /// <summary>
    /// Контейнер, в котором размещаются toggle-элементы группы.
    /// </summary>
    public TLayout ToggleLayout { get; }

    /// <summary>
    /// Дополнительная подпись группы.
    /// </summary>
    public BaseLabel CaptionLabel { get; }

    /// <summary>
    /// Получает или устанавливает фабрику,
    /// создающую визуальный toggle-элемент для указанного значения.
    /// </summary>
    public Func<TItem, IToggleItem> ItemTemplate
    {
        get =>
            _itemTemplate;

        set
        {
            ArgumentNullException.ThrowIfNull(value);

            if (ReferenceEquals(
                    _itemTemplate,
                    value))
            {
                return;
            }

            _itemTemplate =
                value;

            Rebuild();
        }
    }

    /// <summary>
    /// Получает или устанавливает признак отображения
    /// дополнительной подписи <see cref="CaptionLabel"/>.
    /// </summary>
    public bool UseCaptionLabel
    {
        get =>
            _useCaptionLabel;

        set
        {
            if (_useCaptionLabel == value)
                return;

            _useCaptionLabel =
                value;

            UpdateCaptionVisibility();
        }
    }

    /// <summary>
    /// Получает или устанавливает набор значений,
    /// отображаемых внутри toggle-группы.
    /// </summary>
    public IReadOnlyList<TItem> ItemsSource
    {
        get =>
            (IReadOnlyList<TItem>?)GetValue(
                ItemsSourceProperty)
            ?? Array.Empty<TItem>();

        set =>
            SetValue(
                ItemsSourceProperty,
                value);
    }

    /// <summary>
    /// Bindable-свойство для <see cref="ItemsSource"/>.
    /// </summary>
    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IReadOnlyList<TItem>),
            typeof(ToggleGroup<TItem, TLayout>),
            Array.Empty<TItem>(),
            propertyChanged: OnItemsSourceChanged);

    /// <summary>
    /// Получает или устанавливает выбранное значение группы.
    /// </summary>
    public TItem? SelectedItem
    {
        get =>
            (TItem?)GetValue(
                SelectedItemProperty);

        set =>
            SetValue(
                SelectedItemProperty,
                value);
    }

    /// <summary>
    /// Bindable-свойство для <see cref="SelectedItem"/>.
    /// </summary>
    public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(
            nameof(SelectedItem),
            typeof(TItem),
            typeof(ToggleGroup<TItem, TLayout>),
            default(TItem),
            BindingMode.TwoWay,
            propertyChanged: OnSelectedItemChanged);

    #endregion

    #region Events

    /// <summary>
    /// Возникает после изменения выбранного значения группы.
    /// </summary>
    public event EventHandler<ValueChangedEventArgs<TItem>>?
        SelectionChanged;

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт toggle-группу с указанной фабрикой элементов.
    /// </summary>
    /// <param name="componentStore">
    /// Центральное хранилище UI-компонентов и сервисов.
    /// </param>
    /// <param name="itemTemplate">
    /// Фабрика создания toggle-элемента для каждого значения.
    /// </param>
    public ToggleGroup(
        ComponentStore componentStore,
        Func<TItem, IToggleItem> itemTemplate)
    {
        ArgumentNullException.ThrowIfNull(componentStore);
        ArgumentNullException.ThrowIfNull(itemTemplate);

        _componentStore =
            componentStore;

        _itemTemplate =
            itemTemplate;

        _rootGrid =
            new Grid();

        ToggleLayout =
            new TLayout();

        CaptionLabel =
            _componentStore.Base.Label();

        BuildLayout();
        UpdateCaptionVisibility();
        Rebuild();
    }

    #endregion

    #region Layout

    /// <summary>
    /// Формирует внутреннюю структуру toggle-группы.
    /// </summary>
    private void BuildLayout()
    {
        ToggleLayout.HorizontalOptions =
            LayoutOptions.Center;

        CaptionLabel
            .ViewCenter()
            .TextCenter();

        _rootGrid
            .AddRow(0)
            .AddAutoRow();

        _rootGrid
            .AddChild(
                CaptionLabel,
                0,
                0)
            .AddChild(
                ToggleLayout,
                1,
                0);

        Content =
            _rootGrid;
    }

    /// <summary>
    /// Обновляет видимость строки,
    /// содержащей подпись группы.
    /// </summary>
    private void UpdateCaptionVisibility()
    {
        _rootGrid.RowDefinitions[0].Height =
            UseCaptionLabel
                ? GridLength.Auto
                : 0;
    }

    #endregion

    #region Items

    /// <summary>
    /// Полностью перестраивает визуальные элементы группы
    /// на основе текущего <see cref="ItemsSource"/>.
    /// </summary>
    private void Rebuild()
    {
        ToggleLayout.Children.Clear();
        _views.Clear();

        foreach (var item in ItemsSource)
        {
            AddItem(
                item);
        }
    }

    /// <summary>
    /// Создаёт и добавляет toggle-элемент
    /// для указанного значения.
    /// </summary>
    private void AddItem(
        TItem item)
    {
        var toggle =
            ItemTemplate(item);

        ArgumentNullException.ThrowIfNull(toggle);

        ConfigureToggle(
            toggle,
            item);

        _views.Add(
            item,
            toggle);

        ToggleLayout.Children.Add(
            toggle.View);

        toggle.Initialize(
            IsItemSelected(item));
    }

    /// <summary>
    /// Настраивает стандартное визуальное поведение
    /// и обработку выбора toggle-элемента.
    /// </summary>
    private void ConfigureToggle(
        IToggleItem toggle,
        TItem item)
    {
        toggle.AddAction(
            _componentStore.Custom
                .ToggleGroup
                .Styles
                .ToggleBackgroundColor(
                    toggle.View));

        RegisterSelectionHandler(
            toggle.View,
            item);
    }

    #endregion

    #region Selection

    /// <summary>
    /// Регистрирует обработчик выбора элемента.
    /// Для <see cref="Button"/> используется нативное событие Clicked,
    /// для остальных представлений — <see cref="TapGestureRecognizer"/>.
    /// </summary>
    private void RegisterSelectionHandler(
        View view,
        TItem item)
    {
        if (view is Button button)
        {
            button.Clicked +=
                (_, _) => SelectItem(item);

            return;
        }

        var tapGesture =
            new TapGestureRecognizer();

        tapGesture.Tapped +=
            (_, _) => SelectItem(item);

        view.GestureRecognizers.Add(
            tapGesture);
    }

    /// <summary>
    /// Устанавливает указанное значение
    /// как выбранное, если оно ещё не выбрано.
    /// </summary>
    private void SelectItem(
        TItem item)
    {
        if (IsItemSelected(item))
            return;

        SelectedItem =
            item;
    }

    /// <summary>
    /// Проверяет, соответствует ли указанное значение
    /// текущему выбранному элементу группы.
    /// </summary>
    private bool IsItemSelected(
        TItem item)
    {
        return EqualityComparer<TItem>.Default.Equals(
            item,
            SelectedItem);
    }

    /// <summary>
    /// Синхронизирует состояние всех визуальных toggle-элементов
    /// с текущим значением <see cref="SelectedItem"/>.
    /// </summary>
    private void UpdateSelection()
    {
        foreach (var pair in _views)
        {
            pair.Value.IsSelected =
                IsItemSelected(
                    pair.Key);
        }
    }

    #endregion

    #region Bindable Property Callbacks

    /// <summary>
    /// Обрабатывает изменение набора элементов группы.
    /// </summary>
    private static void OnItemsSourceChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var group =
            (ToggleGroup<TItem, TLayout>)bindable;

        group.Rebuild();
    }

    /// <summary>
    /// Обрабатывает изменение выбранного значения,
    /// обновляет визуальные состояния элементов
    /// и вызывает <see cref="SelectionChanged"/>.
    /// </summary>
    private static void OnSelectedItemChanged(
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

        var group =
            (ToggleGroup<TItem, TLayout>)bindable;

        group.UpdateSelection();

        group.SelectionChanged?.Invoke(
            group,
            new ValueChangedEventArgs<TItem>(
                (TItem?)oldValue,
                (TItem?)newValue));
    }

    #endregion
}