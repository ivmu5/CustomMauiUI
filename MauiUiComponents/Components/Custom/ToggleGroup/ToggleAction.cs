namespace MauiUiComponents;

public class ToggleAction<TView>
    where TView : View
{
    public bool ApplyOnInitialize { get; }

    public Action<TView> OnSelected { get; }
    public Action<TView> OnUnselected { get; }

    public ToggleAction(
        Action<TView> onSelected,
        Action<TView> onUnselected,
        bool applyOnInitialize = false)
    {
        OnSelected = onSelected;
        OnUnselected = onUnselected;
        ApplyOnInitialize = applyOnInitialize;
    }
}
