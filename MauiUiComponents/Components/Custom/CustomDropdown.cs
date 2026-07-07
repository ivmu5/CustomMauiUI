namespace MauiUiComponents;

public class CustomDropdown<TItem> : BaseGrid
{
    private readonly IOverlayService _overlayService;

    private readonly BaseBorder<BaseLabel> _selectedLabelBorder;

    private CollectionView? itemsView;

    public IReadOnlyList<TItem> ItemsSource
    {
        get => (IReadOnlyList<TItem>?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    } = Array.Empty<TItem>();
    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IReadOnlyList<TItem>),
            typeof(CustomDropdown<TItem>),
            propertyChanged: OnItemsChanged);

    private static void OnItemsChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var dropdown = (CustomDropdown<TItem>)bindable;

        if (dropdown.itemsView != null)
            dropdown.itemsView.ItemsSource = (IReadOnlyList<TItem>?)newValue;
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
            propertyChanged: OnSelectedChanged);

    private static void OnSelectedChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var dropdown = (CustomDropdown<TItem>)bindable;

        dropdown._selectedLabelBorder.View.Text =
            newValue is TItem item
                ? dropdown.DisplayText(item)
                : string.Empty;
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
            propertyChanged: OnOpenedChanged);

    private static void OnOpenedChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var dropdown = (CustomDropdown<TItem>)bindable;

        if ((bool)newValue)
            dropdown.ShowItems();
        else
            dropdown.HideItems();
    }

    private void ShowItems()
    {
        if (itemsView != null)
            return;

        itemsView = new CollectionView
        {
            ItemsSource = ItemsSource,
            SelectionMode = SelectionMode.Single,
            //ItemTemplate = new DataTemplate(() =>
            //{
            //    var label = new BaseLabel();

            //    label.SetBinding(
            //        Label.TextProperty,
            //        ".",
            //        converter: new DelegateConverter<TItem>(DisplayText));

            //    return label;
            //})
        };
        itemsView.SelectionChanged += OnSelectionChanged;

        _overlayService.AddOverlay(
            itemsView,
            OverlayPlacement.BelowAnchor,
            this);
    }

    private void HideItems()
    {
        if (itemsView == null)
            return;

        _overlayService.RemoveOverlay(itemsView);

        itemsView.SelectionChanged -= OnSelectionChanged;
        itemsView = null;
    }



    public readonly Func<TItem, string> DisplayText;
    //public Func<TItem, View> ItemTemplate

    public CustomDropdown(
        Func<TItem, string> displayText,
        IOverlayService overlayService,
        ComponentStore componentStore)
    {
        DisplayText = displayText;
        _overlayService = overlayService;
        _selectedLabelBorder = componentStore.Base
            .Label()
            .WithBorder(componentStore);


        _selectedLabelBorder.ViewOnTapped(
            (_) => IsOpened = !IsOpened);
        
        BuildLayout();
    }

    private void BuildLayout()
    {
        this.AddChild(_selectedLabelBorder);
    }

    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is TItem item)
        {
            SelectedItem = item;
            IsOpened = false;
        }
    }
}