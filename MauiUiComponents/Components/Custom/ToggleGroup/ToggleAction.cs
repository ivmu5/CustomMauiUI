namespace MauiUiComponents;

public class ToggleAction<TView>
    where TView : View
{
    public readonly Action<TView, ToggleGrid> OnSelected;
    public readonly Action<TView, ToggleGrid> OnUnselected;

    public ToggleAction(
        Action<TView, ToggleGrid> onSelected,
        Action<TView, ToggleGrid> onUnselected)
    {
        OnSelected = onSelected;
        OnUnselected = onUnselected;
    }
}
