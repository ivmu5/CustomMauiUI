using ObjectExtensions;

namespace MauiUiComponents;

public static class EnumView<TEnum>
    where TEnum : struct, Enum
{
    public static CustomDropdown<TEnum> Dropdown(
        IOverlayService overlayService,
        ComponentStore componentStore,
        Func<TEnum, ToggleGrid>? itemTemplate = null)
    {
        itemTemplate ??= item => CreateDefaultToggle(item, componentStore);

        return componentStore.Custom.Dropdown(
            overlayService,
            itemTemplate,
            Enum.GetValues<TEnum>());
    }

    public static ToggleGrid CreateDefaultToggle(
        TEnum item,
        ComponentStore componentStore)
    {
        var toggleGrid = new ToggleGrid();
        var toggleLabel = new ToggleItem<BaseLabel>(
            componentStore.Base.Label());
        toggleLabel.View.Margin = 10;
        toggleLabel.View
            .ViewVerticalCenter()
            .ViewHorizontalStart();

        if (item.GetDisplayAttribute() is
            {
                ResourceType: { } resourceType,
                Name: { } name
            })
        {
            toggleLabel.View.TextBind(
                componentStore.ResourcesStore.GetLocalizationManager(resourceType),
                name);
        }
        else
        {
            toggleLabel.View.Text = item.ToString();
        }

        toggleGrid.AddToggleChild(toggleLabel);

        return toggleGrid;
    }
}
