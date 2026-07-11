namespace MauiUiComponents;

public class ToggleTarget<TView> : IToggleTarget
    where TView : View
{
    private readonly ToggleAction<TView>[] _actions;

    private readonly TView _view;


    public ToggleTarget(
        TView view,
        ToggleAction<TView>[] actions)
    {
        _view = view;
        _actions = actions;
    }

    public void Update(bool isSelected, params ToggleActionTrigger[] triggers)
    {
        var actions = triggers.Length > 0
            ? _actions.Where(a => a.HasType(triggers))
            : _actions;

        foreach (var action in actions)
        {
            if (isSelected)
                action.OnSelected?.Invoke(_view);
            else
                action.OnUnselected?.Invoke(_view);
        }
    }
}
