using MauiUiSettings;
using MauiUiSettings.Resources.Localization.MaterialSymbols;

namespace MauiUiComponents;

public class CustomDropdown<TItem> : ContentView, IDisposable
{
    private readonly ComponentStore _componentStore;
    private readonly IOverlayService _overlayService;

    private readonly BaseBorder<BaseGrid> _rootGridBorder;
    private readonly BaseBorder<BaseButton> _dropdownOpenButtonBorder;
    private readonly BaseBorder<ContentView> _selectedItemContentBorder;

    private BaseBorder<ToggleGroup<FlexLayout>>? _itemsToggleBorder;

    public readonly BaseLabel TextLabel;

    public Func<TItem, ToggleGrid> ItemTemplate { get; }

    #region Bindable Properties

    public IReadOnlyList<TItem> ItemsSource
    {
        get => (IReadOnlyList<TItem>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IReadOnlyList<TItem>),
            typeof(CustomDropdown<TItem>),
            Array.Empty<TItem>(),
            propertyChanged: OnItemsSourceChanged);
    private static void OnItemsSourceChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var dropdown = (CustomDropdown<TItem>)bindable;

        if (dropdown.IsOpened)
        {
            dropdown.HideItems();
            dropdown.ShowItems();
        }
    }


    public TItem? SelectedItem
    {
        get => (TItem?)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(
            nameof(SelectedItem),
            typeof(TItem),
            typeof(CustomDropdown<TItem>),
            default(TItem),
            BindingMode.TwoWay,
            propertyChanged: OnSelectedItemChanged);
    private static void OnSelectedItemChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var dropdown = (CustomDropdown<TItem>)bindable;

        if (newValue is TItem item)
        {
            var toggleGrid = dropdown.ItemTemplate(item);
            dropdown._selectedItemContentBorder.View.Content = toggleGrid.View;

            if (dropdown.IsOpened)
                dropdown.IsOpened = false;
        }
    }


    public bool IsOpened
    {
        get => (bool)GetValue(IsOpenedProperty);
        set => SetValue(IsOpenedProperty, value);
    }
    public static readonly BindableProperty IsOpenedProperty =
        BindableProperty.Create(
            nameof(IsOpened),
            typeof(bool),
            typeof(CustomDropdown<TItem>),
            false,
            BindingMode.TwoWay,
            propertyChanged: OnIsOpenedChanged);
    private static void OnIsOpenedChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var dropdown = (CustomDropdown<TItem>)bindable;

        if ((bool)newValue)
        {
            dropdown.ShowItems();

            dropdown._dropdownOpenButtonBorder.View.TextIconBind(
                dropdown._componentStore,
                nameof(MaterialSymbols.ArrowUp));
        }
        else
        {
            dropdown.HideItems();

            dropdown._dropdownOpenButtonBorder.View.TextIconBind(
                dropdown._componentStore,
                nameof(MaterialSymbols.ArrowDown));
        }
    }

    #endregion

    public CustomDropdown(
        Func<TItem, ToggleGrid> itemTemplate,
        IOverlayService overlayService,
        ComponentStore componentStore)
    {
        ItemTemplate = itemTemplate;
        _overlayService = overlayService;
        _componentStore = componentStore;

        _dropdownOpenButtonBorder = componentStore.Base
            .Button(fontVariant: FontVariant.Icon)
            .TextIconBind(
                _componentStore,
                nameof(MaterialSymbols.ArrowDown))
            .WithBorder(componentStore);

        TextLabel = _componentStore.Base.Label();
        _selectedItemContentBorder = new ContentView().WithBorder(componentStore);
        _rootGridBorder = new BaseGrid().WithBorder(componentStore);

        BuildLayout();
        SubscribeEvents();
    }

    #region Initialization

    private void BuildLayout()
    {
        TextLabel.ViewCenter();

        _rootGridBorder.View
            .AddStarColumn()
            .AddAutoColumn()
            .AddAutoRow()
            .AddStarRow();

        _rootGridBorder.View
            .AddChild(TextLabel, 0, 0, columnSpan: 2)
            .AddChild(_selectedItemContentBorder, 1, 0)
            .AddChild(_dropdownOpenButtonBorder, 1, 1);

        _dropdownOpenButtonBorder.View.TextBind(
            _componentStore.ResourcesStore.MaterialSymbolsManager,
            nameof(MaterialSymbols.ArrowDown));

        Content = _rootGridBorder;
    }

    private void SubscribeEvents()
    {
        _dropdownOpenButtonBorder.View.Clicked += (_, _) => IsOpened = !IsOpened;
        _selectedItemContentBorder.ViewOnTapped(_ => IsOpened = !IsOpened);
    }

    #endregion

    #region Dropdown

    private void ShowItems()
    {
        if (_itemsToggleBorder != null)
            return;

        var toggleGroup = new ToggleGroup<FlexLayout>(_componentStore);
        toggleGroup.ToggleLayout.FlexColumn();

        ToggleGrid? selectedView = null;

        foreach (var item in ItemsSource)
        {
            var toggleGrid = ItemTemplate(item);
            toggleGrid.View.MinimumWidthRequest = _selectedItemContentBorder.Width;

            toggleGrid.AddAction(
                _componentStore.Custom.ToggleGroup.Styles.ToggleBackgroundColor<BaseGrid>(
                    toggleGrid.View,
                    unselectedColor: ColorVariant.None));

            toggleGrid.AddAction(
                new ToggleBehavior<BaseGrid>(
                    toggleGrid.View,
                    _ => SelectedItem = item,
                    _ => { },
                    ToggleTrigger.BusinessAction));

            toggleGroup.AddItem(toggleGrid);

            if (SelectedItem != null && SelectedItem.Equals(item))
                selectedView = toggleGrid;
        }

        toggleGroup.SelectedItem = selectedView;

        _itemsToggleBorder = toggleGroup
            .WithBorder(
                _componentStore,
                backgroundColor: ColorVariant.Blur);

        _overlayService.AddOverlay(
            _itemsToggleBorder,
            OverlayPlacement.BelowAnchor,
            this,
            _ => IsOpened = false);
    }

    private void HideItems()
    {
        if (_itemsToggleBorder == null)
            return;

        _overlayService.RemoveOverlay(_itemsToggleBorder);

        _itemsToggleBorder = null;
    }

    #endregion

    public void Dispose()
    {
        HideItems();
    }
}