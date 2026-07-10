namespace MauiUiComponents;

internal class ToggleTarget<TView> : IToggleTarget
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

        _view.InputTransparent = true;
    }

    public void Update(bool isSelected)
    {
        foreach (var action in _actions)
        {
            if (isSelected) 
                action.OnSelected?.Invoke(_view);
            else
                action.OnUnselected?.Invoke(_view);
        }
    }
}
