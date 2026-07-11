namespace MauiUiComponents;

public interface IToggleTarget
{
    void Update(bool isSelected, params ToggleActionTrigger[] triggers);
}
