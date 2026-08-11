using MauiUiSettings;
using MauiUiSettings.Resources.Localization.MaterialSymbols;

namespace MauiUiComponents;

public class CustomDropdown<TItem> : ContentView, IDisposable
    where TItem : notnull
{
    private readonly ComponentStore _componentStore;
    private readonly IOverlayService _overlayService;

    private readonly BaseBorder<BaseGrid> _rootGridBorder;
    private readonly BaseBorder<BaseButton> _dropdownOpenButtonBorder;
    private readonly BaseBorder<ContentView> _selectedItemContentBorder;

    private BaseBorder<ToggleGroup<TItem, FlexLayout>>? _itemsToggleBorder;

    public readonly BaseLabel CaptionLabel;



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

        dropdown.Rebuild();
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

        dropdown.UpdateSelectedItemContent();

        if (dropdown.IsOpened)
            dropdown.IsOpened = false;
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

    public Func<TItem, IToggleItem> ItemTemplate
    {
        get;
        set
        {
            field = value;
            Rebuild();
        }
    }

    public bool UseCaptionLabel
    {
        get;
        set
        {
            field = value;
            _rootGridBorder.View.RowDefinitions[0].Height = field
                ? GridLength.Auto
                : 0;
        }
    }



    public CustomDropdown(
        Func<TItem, IToggleItem> itemTemplate,
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

        CaptionLabel = _componentStore.Base.Label();
        _selectedItemContentBorder = new ContentView().WithBorder(componentStore);
        _rootGridBorder = new BaseGrid().WithBorder(componentStore);

        BuildLayout();
        SubscribeEvents();
    }

    #region Initialization

    private void BuildLayout()
    {
        CaptionLabel.ViewCenter();

        _rootGridBorder.View
            .AddStarColumn()
            .AddAutoColumn()
            .AddAutoRow()
            .AddStarRow();

        _rootGridBorder.View
            .AddChild(CaptionLabel, 0, 0, columnSpan: 2)
            .AddChild(_selectedItemContentBorder, 1, 0)
            .AddChild(_dropdownOpenButtonBorder, 1, 1);

        _dropdownOpenButtonBorder.View.TextBind(
            _componentStore.LocalizationStore.MaterialSymbolsManager,
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

        var toggleGroup = _componentStore.Custom.ToggleGroup.ToggleGroup<TItem, FlexLayout>(
            ItemsSource,
            item =>
            {
                var toggleItem = ItemTemplate(item);

                toggleItem.View.MinimumWidthRequest = _selectedItemContentBorder.Width;

                toggleItem.AddAction(
                    _componentStore.Custom.ToggleGroup.Styles.ToggleBackgroundColor<View>(
                        toggleItem.View,
                        ColorVariant.Primary,
                        ColorVariant.None));

                return toggleItem;
            },
            SelectedItem);

        toggleGroup.ToggleLayout.FlexColumn();

        this.Bind(
            cd => cd.SelectedItem,
            toggleGroup,
            tg => tg.SelectedItem);

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

    private void Rebuild()
    {
        if (IsOpened)
        {
            HideItems();
            ShowItems();
        }
    }

    public void UpdateSelectedItemContent()
    {
        if (SelectedItem == null)
        {
            _selectedItemContentBorder.View.Content = null;
            return;
        }

        var toggleGrid = ItemTemplate(SelectedItem);
        _selectedItemContentBorder.View.Content = toggleGrid.View;
    }

    #endregion

    public void Dispose()
    {
        HideItems();
    }
}