namespace MauiUiComponents;

internal class ToggleTarget<TView> : IToggleTarget
    where TView : View
{
    private readonly ToggleAction<TView>[] _actions;

    private readonly TView _view;
    private readonly ToggleGrid _parent;


    public ToggleTarget(
        TView view,
        ToggleGrid parent,
        ToggleAction<TView>[] actions)
    {
        _view = view;
        _actions = actions;
        _parent = parent;

        _view.InputTransparent = true;
    }

    public void Update(bool isSelected)
    {
        foreach (var action in _actions)
        {
            if (isSelected)
                action.OnSelected?.Invoke(_view, _parent);
            else
                action.OnUnselected?.Invoke(_view, _parent);
        }
    }
}
