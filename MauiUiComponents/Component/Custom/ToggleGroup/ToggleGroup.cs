using SQLiteStorage;

namespace MauiUiComponents;

public class ToggleGroup<TLayout> : Grid
    where TLayout : Layout, new()
{
    private readonly List<ToggleGrid> _items = new();
    public readonly TLayout ToggleLayout;
    public readonly BaseLabel CaptionLabel;

    public ToggleGrid? SelectedItem { get; private set; }
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
        ToggleLayout = new TLayout();
        CaptionLabel = componentStore.Base.Label();

        ToggleLayout.HorizontalOptions = LayoutOptions.Center;
        CaptionLabel.ViewCenter().TextCenter();

        this.AddRow(0);
        this.AddAutoRow();

        this.AddChild(CaptionLabel);
        this.AddChild(ToggleLayout, 1);
    }

    public void AddItem(ToggleGrid grid, Action? action = null)
    {
        _items.Add(grid);
        ToggleLayout.Children.Add(grid);

        grid.ViewOnTapped((_) =>
        {
            SelectItem(grid);
            action?.Invoke();
        });

        if (SelectedItem == null)
            SelectItem(grid);
    }

    public void SelectItem(ToggleGrid item)
    {
        if (SelectedItem == item)
            return;

        if (!_items.Contains(item))
            return;

        var args = new ValueChangedEventArgs<ToggleGrid>(SelectedItem, item);
        SelectedItem = item;
        foreach (var i in _items)
        {
            i.IsSelected = i == item;
        }
        SelectionChanged?.Invoke(this, args);
    }
}