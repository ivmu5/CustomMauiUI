namespace MauiUiComponents;

public class ToggleGridView<TView> : ToggleGrid
    where TView : View
{
    public readonly ToggleView<TView> ToggleView;

    public ToggleGridView(ToggleView<TView> toggleView)
    {
        ToggleView = toggleView;
        this.AddChild(ToggleView.View);
        ToggleView.AttachToParent(this);
    }
}
