namespace MauiUiComponents;

public interface IToggleItem
{
    View View { get; }
    bool IsSelected { get; set; }

    void UpdateToggleTargets(params ToggleActionTrigger[] triggers);
}
