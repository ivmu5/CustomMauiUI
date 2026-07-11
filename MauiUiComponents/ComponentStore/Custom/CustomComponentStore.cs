using MauiUiSettings;
using System.Numerics;

namespace MauiUiComponents;

public class CustomComponentStore
{
    private readonly ComponentStore _componentStore;

    public readonly ToggleGroupStore ToggleGroup;

    public CustomComponentStore(
        UiServiceStore uiServices,
        ComponentStore componentStore)
    {
        _componentStore = componentStore;
        ToggleGroup = new ToggleGroupStore(uiServices, _componentStore);
    }

    public CustomTextSlider<TValue> TextSlider<TValue>()
        where TValue : INumber<TValue>, IMinMaxValue<TValue>
    {
        var slider = new CustomTextSlider<TValue>(_componentStore);
        return slider;
    }

    public CustomDropdown<TItem> Dropdown<TItem>(
        IOverlayService overlayService,
        Func<TItem, ToggleGrid> itemTemplate,
        params TItem[] items)
    {
        var dropdown = new CustomDropdown<TItem>(itemTemplate, overlayService, _componentStore)
        {
            ItemsSource = items
        };
        return dropdown;
    }
}
