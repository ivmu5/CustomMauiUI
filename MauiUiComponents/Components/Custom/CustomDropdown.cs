using MauiUiSettings;
using MauiUiSettings.Resources.Localization.MaterialSymbols;

namespace MauiUiComponents;

public class CustomDropdown<TItem> : ContentView, IDisposable
{
    //private readonly static List<ToggleAction<View>> _defaultToggleStyle = new List<ToggleAction<View>>()
    //{
    //    new ToggleViewStyle<View>(
    //        x => x.Background,
    //        ColorVariant.Primary,
    //        ColorVariant.None)
    //};

    private readonly ComponentStore _componentStore;
    private readonly IOverlayService _overlayService;

    private readonly BaseBorder<BaseGrid> _rootGridBorder;
    private readonly BaseBorder<BaseButton> _dropdownOpenButtonBorder;
    private readonly BaseBorder<ContentView> _selectedItemContentBorder;

    private BaseBorder<CollectionView>? _itemsViewBorder;
    private BaseBorder<ToggleGroup<FlexLayout>>? _itemsToggleBorder;

    public readonly BaseLabel TextLabel;

    public Func<TItem, View> ItemTemplate { get; }

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

        if (dropdown._itemsViewBorder != null)
            dropdown._itemsViewBorder.View.ItemsSource = (IReadOnlyList<TItem>)newValue;
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

        dropdown._selectedItemContentBorder.View.Content =
            newValue is TItem item
                ? dropdown.ItemTemplate(item)
                : null;
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
        Func<TItem, View> itemTemplate,
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

        _rootGridBorder = new BaseGrid()
            .WithBorder(componentStore);

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
        _dropdownOpenButtonBorder.View.Clicked += OnToggleClicked;

        _selectedItemContentBorder.ViewOnTapped(_ => IsOpened = !IsOpened);
    }

    #endregion

    #region Dropdown

    private void ShowItems()
    {
        if (_itemsToggleBorder != null)
            return;

        var collectionView = new CollectionView
        {
            ItemsSource = ItemsSource,
            SelectedItem = SelectedItem,
            SelectionMode = SelectionMode.Single,
            ItemTemplate = CreateItemTemplate(),
            MinimumWidthRequest = _selectedItemContentBorder.Width
        };

        collectionView.SelectionChanged += OnSelectionChanged;

        _itemsViewBorder = collectionView
            .WithBorder(_componentStore, backgroundColor: ColorVariant.Blur);

        _overlayService.AddOverlay(
            _itemsViewBorder,
            OverlayPlacement.BelowAnchor,
            this,
            v => IsOpened = false);
    }

    private void HideItems()
    {
        if (_itemsToggleBorder == null)
            return;

        _itemsViewBorder.View.SelectionChanged -= OnSelectionChanged;

        _overlayService.RemoveOverlay(_itemsToggleBorder);

        _itemsToggleBorder = null;
    }

    private DataTemplate CreateItemTemplate()
    {
        return new DataTemplate(() =>
        {
            var host = new ContentView();

            host.BindingContextChanged += (_, _) =>
            {
                if (host.BindingContext is TItem item)
                {
                    var view = ItemTemplate(item);
                    view.MinimumWidthRequest = _selectedItemContentBorder.Width;
                    host.Content = view;
                }
            };

            return host;
        });
    }

    #endregion

    #region Events

    private void OnToggleClicked(
        object? sender,
        EventArgs e)
    {
        IsOpened = !IsOpened;
    }

    private void OnSelectionChanged(
        object? sender,
        SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not TItem item)
            return;

        SelectedItem = item;
        IsOpened = false;
    }

    #endregion

    public void Dispose()
    {
        _dropdownOpenButtonBorder.View.Clicked -= OnToggleClicked;

        HideItems();
    }
}