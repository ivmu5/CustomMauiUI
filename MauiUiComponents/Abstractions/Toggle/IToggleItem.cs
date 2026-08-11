namespace MauiUiComponents;

public interface IToggleItem
{
    View View { get; }
    List<IToggleAction> Actions { get; }
    bool IsSelected { get; set; }

    void AddAction(params IToggleAction[] actions);

    void UpdateActions(params ToggleActionTrigger[] triggers);
}
