namespace MauiUiComponents;

public class ToggleGrid : ToggleItem<BaseGrid>
{
    public void AddToggleChild(
        IToggleItem child,
        int row = 0,
        int column = 0,
        int rowSpan = 0,
        int columnSpan = 0)
    {
        child.View.InputTransparent = true;
        child.View.ViewFillHorizontal();

        View.AddChild(child.View, row, column, rowSpan, columnSpan);
        AddAction(child.Actions.ToArray());
    }
}
