namespace MauiUiComponents;

public class ToggleGroup<TItem, TLayout> : ContentView
    where TLayout : Layout, new()
    where TItem : notnull
{
    private readonly ComponentStore _componentStore;

    private readonly Dictionary<TItem, IToggleItem> _views = new();

    private readonly BaseGrid _rootGrid;
    public readonly TLayout ToggleLayout;
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
            typeof(ToggleGroup<TItem, TLayout>),
            Array.Empty<TItem>(),
            propertyChanged: OnItemsSourceChanged);
    private static void OnItemsSourceChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        ((ToggleGroup<TItem, TLayout>)bindable).Rebuild();
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
            typeof(ToggleGroup<TItem, TLayout>),
            default(TItem),
            BindingMode.TwoWay,
            propertyChanged: OnSelectedItemChanged);
    private static void OnSelectedItemChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        if (Equals(oldValue, newValue))
            return;

        var group = (ToggleGroup<TItem, TLayout>)bindable;

        group.UpdateSelection();
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
            _rootGrid.RowDefinitions[0].Height = field
                ? GridLength.Auto
                : 0;
        }
    }

    public ToggleGroup(
        ComponentStore componentStore,
        Func<TItem, IToggleItem> itemTemplate)
    {
        _componentStore = componentStore;

        _rootGrid = new BaseGrid();

        ToggleLayout = new TLayout();
        CaptionLabel = _componentStore.Base.Label();

        ToggleLayout.HorizontalOptions = LayoutOptions.Center;
        CaptionLabel.ViewCenter().TextCenter();

        _rootGrid.AddRow(0);
        _rootGrid.AddAutoRow();

        _rootGrid.AddChild(CaptionLabel);
        _rootGrid.AddChild(ToggleLayout, 1);

        ItemTemplate = itemTemplate;

        Content = _rootGrid;
    }

    private void Rebuild()
    {
        if (ItemTemplate == null)
            return;

        ToggleLayout.Children.Clear();
        _views.Clear();

        foreach (var item in ItemsSource)
        {
            var toggle = ItemTemplate(item);

            toggle.AddAction(
                new ToggleAction<View>(
                    toggle.View,
                    "SetSelectedToggleItem",
                    _ => SelectedItem = item,
                    ToggleActionTrigger.UIStateChange));

            toggle.AddAction(
                _componentStore.Custom.ToggleGroup.Styles.ToggleBackgroundColor(
                    toggle.View));

            toggle.View.ViewOnTapped(
                _ => SelectedItem = item);

            toggle.UpdateActions(ToggleActionTrigger.Initialization);
            ToggleLayout.Children.Add(toggle.View);

            _views[item] = toggle;
        }

        UpdateSelection();
    }

    private void UpdateSelection()
    {
        foreach (var pair in _views)
        {
            pair.Value.IsSelected = EqualityComparer<TItem>.Default.Equals(
                pair.Key,
                SelectedItem);
        }
    }
}