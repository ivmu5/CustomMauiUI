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

        //_view.InputTransparent = true;
    }

    public void Update(bool isSelected, bool onlyInitAction)
    {
        var actions = onlyInitAction
            ? _actions.Where(a => a.ApplyOnInitialize)
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
