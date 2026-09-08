using System.Diagnostics;

namespace MauiUiComponents;

[DebuggerDisplay("{ActionName,nq}")]
public class ToggleAction<TView> : IToggleAction
    where TView : View
{
    private readonly TView _view;

    public string ActionName { get; private set; }

    public IReadOnlySet<ToggleActionTrigger> TriggerTypes { get; }

    public Action<TView> OnSelected { get; }
    public Action<TView>? OnUnselected { get; }

    public ToggleAction(
        TView view,
        string actionName,
        Action<TView> onSelected,
        Action<TView>? onUnselected,
        params ToggleActionTrigger[] types)
    {
        _view = view;
        ActionName = actionName;
        OnSelected = onSelected;
        OnUnselected = onUnselected;
        TriggerTypes = new HashSet<ToggleActionTrigger>(types);
    }

    public ToggleAction(
        TView view,
        string actionName,
        Action<TView> onSelected,
        params ToggleActionTrigger[] triggers)
    : this(view, actionName, onSelected, null, triggers)
    { }

    public bool HasTrigger(ToggleActionTrigger type)
    {
        return TriggerTypes.Contains(type);
    }

    public bool HasTrigger(params ToggleActionTrigger[] types)
    {
        return types.Any(TriggerTypes.Contains);
    }

    public void Execute(bool isSelected)
    {
        if (isSelected)
            OnSelected?.Invoke(_view);
        else
            OnUnselected?.Invoke(_view);
    }
}