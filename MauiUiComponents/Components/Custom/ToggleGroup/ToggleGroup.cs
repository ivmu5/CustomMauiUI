using SQLiteStorage;

namespace MauiUiComponents;

public class ToggleGroup<TLayout> : Grid
    where TLayout : Layout, new()
{
    private readonly ComponentStore _componentStore;

    private readonly List<IToggleItem> _items = new();
    public readonly TLayout ToggleLayout;
    public readonly BaseLabel CaptionLabel;

    public IToggleItem? SelectedItem
    {
        get => (IToggleItem?)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(
            nameof(SelectedItem),
            typeof(IToggleItem),
            typeof(ToggleGroup<TLayout>),
            null,
            propertyChanged: OnSelectedItemChanged);

    private static void OnSelectedItemChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        if (Equals(oldValue, newValue))
            return;

        var control = (ToggleGroup<TLayout>)bindable;

        if (newValue is not IToggleItem value)
            return;

        if (!control._items.Contains(value))
            return;

        foreach (var item in control._items)
        {
            item.IsSelected = item == value;
        }
    }


    public event EventHandler<ValueChangedEventArgs<ToggleGrid>>? SelectionChanged;

    public bool UseCaption
    {
        get;
        set
        {
            field = value;
            if (field)
                RowDefinitions[0].Height = GridLength.Auto;
            else
                RowDefinitions[0].Height = 0;
        }
    } = false;



    public ToggleGroup(ComponentStore componentStore)
    {
        _componentStore = componentStore;

        ToggleLayout = new TLayout();
        CaptionLabel = _componentStore.Base.Label();

        ToggleLayout.HorizontalOptions = LayoutOptions.Center;
        CaptionLabel.ViewCenter().TextCenter();

        this.AddRow(0);
        this.AddAutoRow();

        this.AddChild(CaptionLabel);
        this.AddChild(ToggleLayout, 1);
    }

    public void AddItem(object toggleView, Action? action = null)
    {
        AddItem((IToggleItem)toggleView, action);
    }

    public void AddItem(IToggleItem toggleItem, Action? action = null)
    {
        _items.Add(toggleItem);
        ToggleLayout.Children.Add(toggleItem.View);

        toggleItem.View.ViewOnTapped((_) =>
        {
            SelectedItem = toggleItem;
            action?.Invoke();
        });

        if (SelectedItem == null)
            SelectedItem = toggleItem;

        toggleItem.UpdateToggleTargets(true);
    }
}