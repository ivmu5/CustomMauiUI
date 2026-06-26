using SQLiteStorage;

namespace MauiUiComponents;

public class ToggleGrid : BaseGrid
{
    public bool IsSelected
    {
        get;
        set
        {
            if (field == value)
                return;
            var args = new ValueChangedEventArgs<bool>(field, value);
            field = value;

            SelectedChanged?.Invoke(this, args);
        }
    }

    public event EventHandler<ValueChangedEventArgs<bool>>? SelectedChanged;
}
