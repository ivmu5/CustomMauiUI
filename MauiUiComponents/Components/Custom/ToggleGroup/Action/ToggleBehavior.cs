namespace MauiUiComponents;

public class ToggleBehavior<TView> : IToggleBehavior
    where TView : View, new()
{
    private readonly View _view;

    public IReadOnlySet<ToggleTrigger> TriggerTypes { get; }

    public Action<TView> OnSelected { get; }
    public Action<TView> OnUnselected { get; }

    public ToggleBehavior(
        View view,
        Action<TView> onSelected,
        Action<TView> onUnselected,
        params ToggleTrigger[] types)
    {
        _view = view;
        OnSelected = onSelected;
        OnUnselected = onUnselected;
        TriggerTypes = new HashSet<ToggleTrigger>(types);
    }

    public bool HasTrigger(ToggleTrigger type)
    {
        return TriggerTypes.Contains(type);
    }

    public bool HasTrigger(params ToggleTrigger[] types)
    {
        return types.Any(TriggerTypes.Contains);
    }

    public void Execute(bool isSelected)
    {
        if (isSelected)
            OnSelected?.Invoke((TView)_view);
        else
            OnUnselected?.Invoke((TView)_view);
    }
}