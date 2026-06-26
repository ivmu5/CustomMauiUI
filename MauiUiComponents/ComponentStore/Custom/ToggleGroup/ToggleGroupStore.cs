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

    public ToggleViewStyle<TView> BaseStyle<TView>(Expression<Func<TView, object>> propertyExpression)
        where TView : View
    {
        return new ToggleViewStyle<TView>(
            propertyExpression!,
            ColorVariant.Primary,
            ColorVariant.Secondary);
    }



    public ToggleView<TView> ToggleView<TView>(
        TView view,
        List<ToggleViewStyle<TView>>? styles = null)
        where TView : View
    {
        var toggleView = new ToggleView<TView>(view, _uiServices, styles);

        return toggleView;
    }

    public ToggleGridView<TView> ToggleGridView<TView>(
        TView? view = null,
        List<ToggleViewStyle<TView>>? styles = null)
        where TView : View, new()
    {
        view ??= new();
        view.InputTransparent = true;
        return new ToggleGridView<TView>(ToggleView(view, styles));
    }

    public ToggleGridView<TView> BaseToggleGrid<TView>(
        TView view)
         where TView : View, new()
    {
        view ??= new TView();

        return ToggleGridView(
            view,
            new List<ToggleViewStyle<TView>>()
            {
                BaseStyle<TView>(x => x.Background)
            });
    }



    public ToggleGridView<TView> BaseToggleGridTextView<TView>(
        ILocalizationResourceManager localization,
        string key,
        FontVariant fontVariant = FontVariant.Text)
        where TView : View, ITextComponent, new()
    {
        var view = new TView();
        view.TextStyleBind(_uiServices, fontVariant);
        view.TextBind(localization, key);

        return BaseToggleGrid(view);
    }




    public ToggleGroup<TLayout> ToggleGroup<TLayout>(IEnumerable<ToggleGrid>? items = null, ToggleGrid? selectedItem = null)
        where TLayout : Layout, new()
    {
        var toggleGroup = new ToggleGroup<TLayout>(_componentStore);

        if (items != null)
        {
            foreach (var item in items)
            {
                toggleGroup.AddItem(item);
            }

            if (selectedItem != null)
                toggleGroup.SelectItem(selectedItem);
        }

        return toggleGroup;
    }
}
