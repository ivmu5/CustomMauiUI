namespace MauiUiComponents;

public class CustomDropdown<TItem> : Grid
{
    private readonly Border _header;
    private readonly Label _selectedLabel;
    private readonly CollectionView _itemsView;

    private IReadOnlyList<TItem> _items = [];
    private TItem? _selectedItem;
    private bool _isOpened;

    public CustomDropdown()
    {
        RowDefinitions =
        [
            new(GridLength.Auto),
            new(GridLength.Auto)
        ];

        _selectedLabel = new Label();

        _header = new Border
        {
            Padding = 12,
            Content = _selectedLabel
        };

        _itemsView = new CollectionView
        {
            IsVisible = false,
            SelectionMode = SelectionMode.Single
        };

        _itemsView.SelectionChanged += OnSelectionChanged;

        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) => IsOpened = !IsOpened;

        _header.GestureRecognizers.Add(tap);

        Add(_header);
        Add(_itemsView);

        this.SetRow(_itemsView, 1);
    }

    public event EventHandler<TItem>? SelectionChanged;

    public Func<TItem, string> DisplayText { get; set; }
        = item => item?.ToString() ?? "";

    public IReadOnlyList<TItem> ItemsSource
    {
        get => _items;
        set
        {
            _items = value;
            _itemsView.ItemsSource = value;
        }
    }

    public TItem? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (EqualityComparer<TItem>.Default.Equals(_selectedItem, value))
                return;

            _selectedItem = value;

            _selectedLabel.Text =
                value is null
                    ? ""
                    : DisplayText(value);

            _itemsView.SelectedItem = value;
        }
    }

    public bool IsOpened
    {
        get => _isOpened;
        set
        {
            if (_isOpened == value)
                return;

            _isOpened = value;

            _itemsView.IsVisible = value;
        }
    }

    private void OnSelectionChanged(
        object? sender,
        SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not TItem item)
            return;

        SelectedItem = item;
        IsOpened = false;

        SelectionChanged?.Invoke(this, item);
    }
}
