namespace MauiUiComponents;

public interface IToggleItem
{
    View View { get; }
    List<IToggleBehavior> Actions { get; }
    bool IsSelected { get; set; }

    void AddAction(params IToggleBehavior[] actions);

    void UpdateActions(params ToggleTrigger[] triggers);
}
