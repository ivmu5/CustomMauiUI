using MauiUiSettings;
using System.Linq.Expressions;

namespace MauiUiComponents;

public class ToggleGroupStore
{
    private readonly UiServiceStore _uiServices;
    private readonly ComponentStore _componentStore;

    public ToggleGroupStore(
        UiServiceStore uiServices,
        ComponentStore componentStore)
    {
        _uiServices = uiServices;
        _componentStore = componentStore;
    }

    public ToggleAction<TView> ToggleColorAction<TView>(
        Expression<Func<TView, object?>> propertyExpression,
        ColorVariant selectedColor = ColorVariant.Primary,
        ColorVariant unselectedColor = ColorVariant.Secondary)
        where TView : View
    {
        return new(
            (view) => view.ColorBind(_uiServices, propertyExpression, selectedColor),
            (view) => view.ColorBind(_uiServices, propertyExpression, unselectedColor),
            true);
    }

    public ToggleAction<TView> ToggleBackgroundColorAction<TView>(
        ColorVariant selectedColor = ColorVariant.Primary,
        ColorVariant unselectedColor = ColorVariant.Secondary)
        where TView : View
    {
        return ToggleColorAction<TView>(
            x => x.Background,
            selectedColor,
            unselectedColor);
    }



    public ToggleItem<TView> ToggleView<TView>(Action<ToggleItem<TView>> viewTemplate)
        where TView : View, new()
    {
        var toggleView = new ToggleItem<TView>();
        viewTemplate(toggleView);
        return toggleView;
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
        params ToggleAction<TView>[] actions)
        where TView : View, ITextComponent, new()
    {
        var toggleView = new ToggleItem<TView>();
        toggleView.View
            .ViewCenter()
            .TextStyleBind(_uiServices, FontVariant.Text)
            .TextBind(
                localizationManager,
                localizationKey);

        var toggleTarget = new ToggleTarget<TView>(
            toggleView.View,
            actions);
        toggleView.AddToggleTarget(toggleTarget);

        return toggleView;
    }

    public ToggleItem<TView> BaseIconToggleView<TView>(
        string iconKey,
        params ToggleAction<TView>[] actions)
        where TView : View, ITextComponent, new()
    {
        var toggleIconView = new ToggleItem<TView>();
        toggleIconView.View
            .ViewCenter()
            .TextStyleBind(_uiServices, FontVariant.Icon)
            .TextIconBind(
                _componentStore,
                iconKey);

        var toggleTarget = new ToggleTarget<TView>(
            toggleIconView.View,
            actions);
        toggleIconView.AddToggleTarget(toggleTarget);

        return toggleIconView;
    }
}
