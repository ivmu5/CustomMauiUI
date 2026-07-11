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
            (view, grid) => view.ColorBind(_uiServices, propertyExpression, selectedColor),
            (view, grid) => view.ColorBind(_uiServices, propertyExpression, unselectedColor));
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

    public ToggleAction<TView> ToggleGridBackgroundColorAction<TView>(
        ColorVariant selectedColor = ColorVariant.Primary,
        ColorVariant unselectedColor = ColorVariant.Secondary)
        where TView : View
    {
        Expression<Func<ToggleGrid, object?>> propertyExpression = x => x.Background;
        return new(
            (view, grid) => grid.ColorBind(_uiServices, propertyExpression, selectedColor),
            (view, grid) => grid.ColorBind(_uiServices, propertyExpression, unselectedColor)); ;
    }



    public ToggleGrid ToggleGrid(Action<ToggleGrid> viewTemplate)
    {
        var toggleGrid = new ToggleGrid();
        viewTemplate(toggleGrid);
        return toggleGrid;
    }

    public ToggleGroup<TLayout> ToggleGroup<TLayout>()
        where TLayout : Layout, new()
    {
        return new ToggleGroup<TLayout>(_componentStore);
    }

    public ToggleGroup<TLayout> BaseToggleGroup<TLayout, TItem>(
        IEnumerable<TItem> items,
        Action<TItem, ToggleGrid> viewTemplate,
        ToggleGrid? selectedItem = null)
        where TLayout : Layout, new()
    {
        var toggleGroup = ToggleGroup<TLayout>();

        foreach (var item in items)
        {
            var toggleGrid = ToggleGrid(
                grid => viewTemplate(item, grid));
            toggleGroup.AddItem(toggleGrid);
        }

        if (selectedItem != null)
            toggleGroup.SelectItem(selectedItem);

        return toggleGroup;
    }

    public ToggleGrid BaseTextToggleGrid<TView>(
        ILocalizationResourceManager localizationManager,
        string localizationKey,
        params ToggleAction<TView>[] actions)
        where TView : View, ITextComponent, new()
    {
        var toggleGrid = _componentStore.Custom.ToggleGroup.ToggleGrid(
            grid =>
            {
                var view = new TView();
                view
                    .ViewCenter()
                    .TextStyleBind(_uiServices, FontVariant.Text)
                    .TextBind(
                        localizationManager,
                        localizationKey);
                grid.AddToggleChild(view, actions: actions);
            });

        return toggleGrid;
    }

    public ToggleGrid BaseIconToggleGrid<TView>(
        string iconKey,
        params ToggleAction<TView>[] actions)
        where TView : View, ITextComponent, new()
    {
        var toggleGrid = _componentStore.Custom.ToggleGroup.ToggleGrid(
            grid =>
            {
                var view = new TView();
                view
                    .ViewCenter()
                    .TextStyleBind(_uiServices, FontVariant.Icon)
                    .TextIconBind(
                        _componentStore,
                        iconKey);
                grid.AddToggleChild(view, actions: actions);
            });

        return toggleGrid;
    }
}
