namespace MauiUiComponents;

public class ToggleAction<TView>
    where TView : View
{
    public readonly Action<TView> OnSelected;
    public readonly Action<TView> OnUnselected;

    public ToggleAction(
        Action<TView> onSelected,
        Action<TView> onUnselected)
    {
        OnSelected = onSelected;
        OnUnselected = onUnselected;
    }
}
