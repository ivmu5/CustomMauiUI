using MauiUiSettings;

namespace MauiUiComponents;

public class ToggleGroupStore
{
    private readonly UiServiceStore _uiServices;
    private readonly ComponentStore _componentStore;

    public ToggleGroupStyleStore Styles { get; }



    public ToggleGroupStore(
        UiServiceStore uiServices,
        ComponentStore componentStore)
    {
        _uiServices = uiServices;
        _componentStore = componentStore;

        Styles = new ToggleGroupStyleStore(_uiServices);
    }


    public ToggleGroup<TLayout> ToggleGroup<TLayout>()
        where TLayout : Layout, new()
    {
        return new ToggleGroup<TLayout>(_componentStore);
    }

    public ToggleGroup<TLayout> BaseToggleGroup<TLayout, TItem>(
        IEnumerable<TItem> items,
        Action<TItem, ToggleGrid> viewTemplate,
        TItem? selectedItem = default)
        where TLayout : Layout, new()
    {
        var toggleGroup = ToggleGroup<TLayout>();

        ToggleGrid? selectedView = null;

        foreach (var item in items)
        {
            var toggleGrid = new ToggleGrid();
            viewTemplate(item, toggleGrid);

            toggleGroup.AddItem(toggleGrid);

            if (Equals(item, selectedItem))
                selectedView = toggleGrid;
        }

        if (selectedView != null)
            toggleGroup.SelectedItem = selectedView;

        return toggleGroup;
    }

    public ToggleItem<TView> BaseTextToggleView<TView>(
        ILocalizationResourceManager localizationManager,
        string localizationKey,
        params ToggleBehavior<TView>[] actions)
        where TView : View, ITextComponent, new()
    {
        var toggleView = new ToggleItem<TView>();
        toggleView.View
            .ViewCenter()
            .TextStyleBind(_uiServices, FontVariant.Text)
            .TextBind(
                localizationManager,
                localizationKey);

        toggleView.AddAction(actions);

        return toggleView;
    }

    public ToggleItem<TView> BaseIconToggleView<TView>(
        string iconKey,
        params ToggleBehavior<TView>[] actions)
        where TView : View, ITextComponent, new()
    {
        var toggleIconView = new ToggleItem<TView>();
        toggleIconView.View
            .ViewCenter()
            .TextStyleBind(_uiServices, FontVariant.Icon)
            .TextIconBind(
                _componentStore,
                iconKey);

        toggleIconView.AddAction(actions);

        return toggleIconView;
    }
}
