namespace MauiUiComponents;

public interface IToggleAction
{
    string ActionName { get; }

    IReadOnlySet<ToggleActionTrigger> TriggerTypes { get; }

    void Execute(bool isSelected);

    bool HasTrigger(ToggleActionTrigger trigger);
    bool HasTrigger(params ToggleActionTrigger[] triggers);
}