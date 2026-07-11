namespace MauiUiComponents;

public class ToggleAction<TView>
    where TView : View
{
    public IReadOnlySet<ToggleActionTrigger> Types { get; }

    public Action<TView> OnSelected { get; }
    public Action<TView> OnUnselected { get; }

    public ToggleAction(
        Action<TView> onSelected,
        Action<TView> onUnselected,
        params ToggleActionTrigger[] types)
    {
        OnSelected = onSelected;
        OnUnselected = onUnselected;
        Types = new HashSet<ToggleActionTrigger>(types);
    }

    public bool HasType(ToggleActionTrigger type)
    {
        return Types.Contains(type);
    }

    public bool HasType(params ToggleActionTrigger[] types)
    {
        return types.Any(Types.Contains);
    }
}