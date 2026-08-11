using MauiUiSettings;
using System.Numerics;

namespace MauiUiComponents;

public class CustomComponentStore
{
    private readonly ComponentStore _componentStore;

    public readonly ToggleGroupStore ToggleGroup;

    public CustomComponentStore(
        ComponentStore componentStore)
    {
        _componentStore = componentStore;
        ToggleGroup = new ToggleGroupStore(_componentStore);
    }

    public CustomTextSlider<TValue> TextSlider<TValue>()
        where TValue : INumber<TValue>, IMinMaxValue<TValue>
    {
        var slider = new CustomTextSlider<TValue>(_componentStore);
        return slider;
    }

    public CustomDropdown<TItem> Dropdown<TItem>(
        IOverlayService overlayService,
        Func<TItem, IToggleItem> itemTemplate,
        params TItem[] items)
        where TItem : notnull
    {
        var dropdown = new CustomDropdown<TItem>(itemTemplate, overlayService, _componentStore)
        {
            ItemsSource = items
        };
        return dropdown;
    }
}
