using MauiUiSettings;
using MauiUiSettings.Resources.Localization.MaterialSymbols;
using SQLiteStorage;

namespace MauiUiComponents;

/// <summary>
/// Представляет выпадающий список с пользовательским шаблоном элементов,
/// использующий <see cref="ToggleGroup{TItem, TLayout}"/> для выбора значения
/// и <see cref="IOverlayService"/> для отображения списка.
/// </summary>
/// <typeparam name="TItem">
/// Тип значения, отображаемого и выбираемого в dropdown.
/// </typeparam>
public class CustomDropdown<TItem> :
    ContentView,
    IDisposable
    where TItem : notnull
{
    #region Fields

    private readonly ComponentStore _componentStore;
    private readonly IOverlayService _overlayService;

    private readonly BaseBorder<Grid> _rootGridBorder;

    private readonly BaseBorder<BaseButton>
        _dropdownOpenButtonBorder;

    private readonly BaseBorder<ContentView>
        _selectedItemContentBorder;

    private readonly TapGestureRecognizer
        _selectedItemTapGesture;

    private BaseBorder<ToggleGroup<TItem, FlexLayout>>?
        _itemsToggleBorder;

    private ToggleGroup<TItem, FlexLayout>?
        _itemsToggleGroup;

    private Func<TItem, IToggleItem> _itemTemplate;

    private bool _useCaptionLabel;
    private bool _disposed;

    #endregion

    #region Properties

    /// <summary>
    /// Получает дополнительную подпись dropdown-компонента.
    /// </summary>
    public BaseLabel CaptionLabel { get; }

    /// <summary>
    /// Получает или устанавливает фабрику,
    /// создающую визуальное представление элемента.
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

            RebuildItems();
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

    #endregion

    #region Events

    /// <summary>
    /// Возникает после изменения выбранного значения dropdown.
    /// </summary>
    public event EventHandler<ValueChangedEventArgs<TItem>>?
        SelectionChanged;

    #endregion

    #region Bindable Properties

    /// <summary>
    /// Получает или устанавливает набор доступных значений.
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
            typeof(CustomDropdown<TItem>),
            Array.Empty<TItem>(),
            propertyChanged: OnItemsSourceChanged);

    /// <summary>
    /// Получает или устанавливает выбранное значение.
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
            typeof(CustomDropdown<TItem>),
            default(TItem),
            BindingMode.TwoWay,
            propertyChanged: OnSelectedItemChanged);

    /// <summary>
    /// Получает или устанавливает состояние открытого списка.
    /// </summary>
    public bool IsOpened
    {
        get =>
            (bool)GetValue(
                IsOpenedProperty);

        set =>
            SetValue(
                IsOpenedProperty,
                value);
    }

    /// <summary>
    /// Bindable-свойство для <see cref="IsOpened"/>.
    /// </summary>
    public static readonly BindableProperty IsOpenedProperty =
        BindableProperty.Create(
            nameof(IsOpened),
            typeof(bool),
            typeof(CustomDropdown<TItem>),
            false,
            BindingMode.TwoWay,
            propertyChanged: OnIsOpenedChanged);

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт dropdown-компонент.
    /// </summary>
    /// <param name="overlayService">
    /// Сервис отображения выпадающего списка поверх страницы.
    /// </param>
    /// <param name="componentStore">
    /// Центральное хранилище UI-компонентов и сервисов.
    /// </param>
    /// <param name="itemTemplate">
    /// Фабрика визуального представления каждого элемента.
    /// </param>
    public CustomDropdown(
        IOverlayService overlayService,
        ComponentStore componentStore,
        Func<TItem, IToggleItem> itemTemplate)
    {
        ArgumentNullException.ThrowIfNull(overlayService);
        ArgumentNullException.ThrowIfNull(componentStore);
        ArgumentNullException.ThrowIfNull(itemTemplate);

        _overlayService =
            overlayService;

        _componentStore =
            componentStore;

        _itemTemplate =
            itemTemplate;

        _dropdownOpenButtonBorder =
            _componentStore.Base
                .Button(
                    fontVariant: FontVariant.Icon)
                .TextIconBind(
                    _componentStore,
                    nameof(MaterialSymbols.ArrowDown))
                .WithBorder(
                    _componentStore);

        CaptionLabel =
            _componentStore.Base.Label();

        _selectedItemContentBorder =
            new ContentView()
                .WithBorder(
                    _componentStore);

        _rootGridBorder =
            new Grid()
                .WithBorder(
                    _componentStore);

        _selectedItemTapGesture =
            new TapGestureRecognizer();

        BuildLayout();
        UpdateCaptionVisibility();
        SubscribeEvents();
    }

    #endregion

    #region Initialization

    /// <summary>
    /// Формирует постоянную визуальную структуру dropdown-компонента.
    /// </summary>
    private void BuildLayout()
    {
        CaptionLabel.ViewCenter();

        _rootGridBorder.View
            .AddStarColumn()
            .AddAutoColumn()
            .AddAutoRow()
            .AddStarRow();

        _rootGridBorder.View
            .AddChild(
                CaptionLabel,
                0,
                0,
                columnSpan: 2)
            .AddChild(
                _selectedItemContentBorder,
                1,
                0)
            .AddChild(
                _dropdownOpenButtonBorder,
                1,
                1);

        Content =
            _rootGridBorder;
    }

    /// <summary>
    /// Регистрирует обработчики взаимодействия
    /// с постоянной частью dropdown.
    /// </summary>
    private void SubscribeEvents()
    {
        _dropdownOpenButtonBorder
            .View
            .Clicked += OnOpenButtonClicked;

        _selectedItemTapGesture.Tapped +=
            OnSelectedItemTapped;

        _selectedItemContentBorder
            .GestureRecognizers
            .Add(
                _selectedItemTapGesture);
    }

    /// <summary>
    /// Обновляет видимость строки с подписью dropdown.
    /// </summary>
    private void UpdateCaptionVisibility()
    {
        _rootGridBorder
            .View
            .RowDefinitions[0]
            .Height =
                UseCaptionLabel
                    ? GridLength.Auto
                    : 0;
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Переключает состояние dropdown после нажатия
    /// на кнопку раскрытия списка.
    /// </summary>
    private void OnOpenButtonClicked(
        object? sender,
        EventArgs e)
    {
        Toggle();
    }

    /// <summary>
    /// Переключает состояние dropdown после нажатия
    /// на область текущего выбранного значения.
    /// </summary>
    private void OnSelectedItemTapped(
        object? sender,
        TappedEventArgs e)
    {
        Toggle();
    }

    /// <summary>
    /// Переносит выбор из внутренней toggle-группы
    /// в собственное свойство <see cref="SelectedItem"/>.
    /// </summary>
    private void OnToggleSelectionChanged(
        object? sender,
        ValueChangedEventArgs<TItem> e)
    {
        if (sender is not ToggleGroup<TItem, FlexLayout> toggleGroup)
            return;

        var selectedItem =
            toggleGroup.SelectedItem;

        if (selectedItem is null)
            return;

        /*
         * Не используем конкретное имя свойства New/NewValue
         * из ValueChangedEventArgs: единственным источником состояния
         * здесь является сама ToggleGroup.
         */
        SelectedItem =
            selectedItem;
    }

    #endregion

    #region Dropdown State

    /// <summary>
    /// Открывает выпадающий список.
    /// </summary>
    public void Open()
    {
        ObjectDisposedException.ThrowIf(
            _disposed,
            this);

        IsOpened =
            true;
    }

    /// <summary>
    /// Закрывает выпадающий список.
    /// </summary>
    public void Close()
    {
        ObjectDisposedException.ThrowIf(
            _disposed,
            this);

        IsOpened =
            false;
    }

    /// <summary>
    /// Переключает текущее состояние dropdown.
    /// </summary>
    public void Toggle()
    {
        ObjectDisposedException.ThrowIf(
            _disposed,
            this);

        IsOpened =
            !IsOpened;
    }

    /// <summary>
    /// Создаёт список элементов и отображает его
    /// через <see cref="IOverlayService"/>.
    /// </summary>
    private void ShowItems()
    {
        if (_itemsToggleBorder is not null)
            return;

        var toggleGroup =
            _componentStore.Custom
                .ToggleGroup
                .ToggleGroup<TItem, FlexLayout>(
                    ItemsSource,
                    CreateDropdownToggle,
                    SelectedItem);

        toggleGroup.ToggleLayout
            .FlexColumn();

        toggleGroup.SelectionChanged +=
            OnToggleSelectionChanged;

        _itemsToggleGroup =
            toggleGroup;

        _itemsToggleBorder =
            toggleGroup.WithBorder(
                _componentStore,
                backgroundColor: ColorVariant.Blur);

        _overlayService.AddOverlay(
            _itemsToggleBorder,
            OverlayPlacement.BelowAnchor,
            this,
            _ => Close());
    }

    /// <summary>
    /// Удаляет список элементов и освобождает
    /// связанные с ним обработчики событий.
    /// </summary>
    private void HideItems()
    {
        if (_itemsToggleBorder is null)
            return;

        if (_itemsToggleGroup is not null)
        {
            _itemsToggleGroup.SelectionChanged -=
                OnToggleSelectionChanged;
        }

        _overlayService.RemoveOverlay(
            _itemsToggleBorder);

        _itemsToggleGroup =
            null;

        _itemsToggleBorder =
            null;
    }

    /// <summary>
    /// Перестраивает содержимое dropdown
    /// после изменения набора элементов или шаблона.
    /// </summary>
    private void RebuildItems()
    {
        UpdateSelectedItemContent();

        if (!IsOpened)
            return;

        HideItems();
        ShowItems();
    }

    #endregion

    #region Dropdown Items

    /// <summary>
    /// Создаёт toggle-элемент для выпадающего списка
    /// и применяет к нему оформление dropdown.
    /// </summary>
    private IToggleItem CreateDropdownToggle(
        TItem item)
    {
        var toggleItem =
            ItemTemplate(item);

        ArgumentNullException.ThrowIfNull(toggleItem);

        toggleItem.View.MinimumWidthRequest =
            _selectedItemContentBorder.Width;

        /*
         * Действие добавляется до стандартного действия ToggleGroup
         * с тем же именем. ToggleItem хранит действия по ActionName,
         * поэтому этот стиль намеренно переопределяет стандартный:
         *
         * selected   -> Primary
         * unselected -> None
         */
        toggleItem.AddAction(
            _componentStore.Custom
                .ToggleGroup
                .Styles
                .ToggleBackgroundColor(
                    toggleItem.View,
                    ColorVariant.Primary,
                    ColorVariant.None));

        return toggleItem;
    }

    #endregion

    #region Selected Item

    /// <summary>
    /// Перестраивает визуальное представление
    /// текущего выбранного значения.
    /// </summary>
    public void UpdateSelectedItemContent()
    {
        var selectedItem =
            SelectedItem;

        if (selectedItem is null)
        {
            _selectedItemContentBorder
                .View
                .Content = null;

            return;
        }

        var toggleItem =
            ItemTemplate(
                selectedItem);

        ArgumentNullException.ThrowIfNull(toggleItem);

        /*
         * Выбранное представление используется только как preview.
         * Нажатие должно обрабатываться контейнером dropdown,
         * а не самим вложенным toggle-компонентом.
         */
        toggleItem.View.InputTransparent =
            true;

        _selectedItemContentBorder
            .View
            .Content =
                toggleItem.View;
    }

    #endregion

    #region Appearance

    /// <summary>
    /// Изменяет иконку кнопки раскрытия dropdown.
    /// </summary>
    private void SetDropdownIcon(
        string iconName)
    {
        _dropdownOpenButtonBorder
            .View
            .TextIconBind(
                _componentStore,
                iconName);
    }

    #endregion

    #region Bindable Property Callbacks

    /// <summary>
    /// Обрабатывает изменение набора доступных элементов.
    /// </summary>
    private static void OnItemsSourceChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var dropdown =
            (CustomDropdown<TItem>)bindable;

        dropdown.RebuildItems();
    }

    /// <summary>
    /// Обрабатывает изменение выбранного значения,
    /// обновляет preview, закрывает список и вызывает событие выбора.
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

        var dropdown =
            (CustomDropdown<TItem>)bindable;

        dropdown.UpdateSelectedItemContent();

        if (dropdown.IsOpened)
        {
            dropdown.IsOpened =
                false;
        }

        dropdown.SelectionChanged?.Invoke(
            dropdown,
            new ValueChangedEventArgs<TItem>(
                (TItem?)oldValue,
                (TItem?)newValue));
    }

    /// <summary>
    /// Обрабатывает открытие или закрытие dropdown
    /// и синхронизирует состояние иконки.
    /// </summary>
    private static void OnIsOpenedChanged(
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

        var dropdown =
            (CustomDropdown<TItem>)bindable;

        if ((bool)newValue)
        {
            dropdown.ShowItems();

            dropdown.SetDropdownIcon(
                nameof(MaterialSymbols.ArrowUp));

            return;
        }

        dropdown.HideItems();

        dropdown.SetDropdownIcon(
            nameof(MaterialSymbols.ArrowDown));
    }

    #endregion

    #region Dispose

    /// <summary>
    /// Освобождает обработчики событий
    /// и удаляет открытый overlay dropdown.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        /*
         * HideItems вызываем до установки _disposed,
         * поскольку закрываем внутренние ресурсы напрямую,
         * не используя публичный Close().
         */
        HideItems();

        _dropdownOpenButtonBorder
            .View
            .Clicked -= OnOpenButtonClicked;

        _selectedItemTapGesture.Tapped -=
            OnSelectedItemTapped;

        _selectedItemContentBorder
            .GestureRecognizers
            .Remove(
                _selectedItemTapGesture);

        _disposed =
            true;

        GC.SuppressFinalize(
            this);
    }

    #endregion
}