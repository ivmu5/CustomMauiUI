using ObjectExtensions;

namespace MauiUiComponents;

public static class EnumView<TEnum>
    where TEnum : struct, Enum
{
    public static ToggleGroup<TEnum, TLayout> ToggleGroup<TLayout>(
        ComponentStore componentStore,
        Func<TEnum, IToggleItem>? itemTemplate = null)
        where TLayout : Layout, new()
    {
        itemTemplate ??= item => CreateDefaultToggle<BaseButton>(
            item, 
            componentStore,
            () => componentStore.Base.Button());

        return componentStore.Custom.ToggleGroup.ToggleGroup<TEnum, TLayout>(
            Enum.GetValues<TEnum>(),
            itemTemplate);
    }

    public static CustomDropdown<TEnum> Dropdown(
        IOverlayService overlayService,
        ComponentStore componentStore,
        Func<TEnum, IToggleItem>? itemTemplate = null)
    {
        itemTemplate ??= item =>
        {
            var toggleItem = CreateDefaultToggle(
                item,
                componentStore,
                () => componentStore.Base.Label());

            toggleItem.View
                .TextLeft()
                .ViewVerticalCenter()
                .Padding = 10;

            return toggleItem;
        };


        return componentStore.Custom.Dropdown(
            overlayService,
            itemTemplate,
            Enum.GetValues<TEnum>());
    }

    public static ToggleItem<TToggleView> CreateDefaultToggle<TToggleView>(
        TEnum item,
        ComponentStore componentStore,
        Func<TToggleView>? viewTemplate = null)
        where TToggleView : View, ITextComponent, new()
    {
        var toggleView = new ToggleItem<TToggleView>(viewTemplate?.Invoke());

        if (item.GetDisplayAttribute() is
            {
                ResourceType: { } resourceType,
                Name: { } name
            })
        {
            toggleView.View.TextBind(
                componentStore.LocalizationStore.GetLocalizationManager(resourceType),
                name);
        }
        else
        {
            toggleView.View.Text = item.ToString();
        }


        return toggleView;
    }
}
