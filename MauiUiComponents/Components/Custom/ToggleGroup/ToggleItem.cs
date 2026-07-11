namespace MauiUiComponents;

public class ToggleItem<TView> : BindableObject, IToggleItem
    where TView : View, new()
{
    public TView View { get; init; } = new TView();
    View IToggleItem.View => View;

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
            typeof(ToggleItem<TView>),
            false,
            propertyChanged: OnIsSelectedChanged);

    private static void OnIsSelectedChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        if (Equals(oldValue, newValue))
            return;

        var control = (ToggleItem<TView>)bindable;
        var value = (bool)newValue;

        control.UpdateToggleTargets(false);
    }

    public void AddToggleTarget(IToggleTarget target)
    {
        if (!_toggleTargets.Contains(target))
            _toggleTargets.Add(target);
    }

    public void UpdateToggleTargets(bool onlyInitAction)
    {
        foreach (var target in _toggleTargets)
        {
            target.Update(IsSelected, onlyInitAction);
        }
    }
}
