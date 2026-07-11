namespace MauiUiComponents;

public class ToggleItem<TView> : BindableObject, IToggleItem
    where TView : View, new()
{
    public TView View { get; init; } = new TView();
    View IToggleItem.View => View;

    public List<IToggleBehavior> Actions { get; } = new();

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

        var item = (ToggleItem<TView>)bindable;

        item.UpdateActions(
            ToggleTrigger.UIStateChange,
            ToggleTrigger.BusinessAction);
    }

    public void AddAction(params IToggleBehavior[] actions)
    {
        foreach (var action in actions)
        {
            if (!Actions.Contains(action))
                Actions.Add(action);
        }
    }


    public void UpdateActions(params ToggleTrigger[] triggers)
    {
        var actions = triggers.Length == 0
            ? Actions
            : Actions.Where(a => a.HasTrigger(triggers));

        foreach (var action in actions)
        {
            action.Execute(IsSelected);
        }
    }
}
