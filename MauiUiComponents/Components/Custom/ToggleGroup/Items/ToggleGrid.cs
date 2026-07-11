namespace MauiUiComponents;

public class ToggleGrid : ToggleItem<BaseGrid>
{
    public void AddToggleChild<TView>(
        TView child,
        int row = 0,
        int column = 0,
        int rowSpan = 0,
        int columnSpan = 0,
        params ToggleAction<TView>[] actions)
        where TView : View
    {
        var toggleTarget = new ToggleTarget<TView>(
            child,
            actions);

        child.InputTransparent = true;

        View.AddChild(child, row, column, rowSpan, columnSpan);
        AddToggleTarget(toggleTarget);

        child.ViewFillHorizontal();

        toggleTarget.Update(IsSelected, ToggleActionTrigger.Initialization);
    }
}
