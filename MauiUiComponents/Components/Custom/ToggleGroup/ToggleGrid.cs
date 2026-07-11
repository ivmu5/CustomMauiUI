namespace MauiUiComponents;

public class ToggleGrid : BaseGrid
{
    private readonly List<IToggleTarget> _toggleTargets = new();

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }
    public static readonly BindableProperty IsSelectedProperty =
        BindableProperty.Create(
            nameof(IsSelected),
            typeof(bool),
            typeof(ToggleGrid),
            false,
            propertyChanged: OnIsSelectedChanged);
    private static void OnIsSelectedChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        if (Equals(oldValue, newValue))
            return;

        var control = (ToggleGrid)bindable;
        var value = (bool)newValue;

        foreach (var target in control._toggleTargets)
        {
            target.Update(value);
        }
    }



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
            this,
            actions);

        this.AddChild(child, row, column, rowSpan, columnSpan);
        _toggleTargets.Add(toggleTarget);

        child.ViewFillHorizontal();

        toggleTarget.Update(IsSelected);
    }
}
