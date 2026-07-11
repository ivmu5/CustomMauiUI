namespace MauiUiComponents;

public interface IToggleBehavior
{
    IReadOnlySet<ToggleTrigger> TriggerTypes { get; }

    void Execute(bool isSelected);

    bool HasTrigger(ToggleTrigger trigger);
    bool HasTrigger(params ToggleTrigger[] triggers);
}