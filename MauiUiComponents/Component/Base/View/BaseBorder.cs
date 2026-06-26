namespace MauiUiComponents;

public class BaseBorder<TView> : Border
    where TView : View
{
    public readonly TView View;

    public BaseBorder(TView view)
    {
        View = view;
        Content = view;
    }
}
