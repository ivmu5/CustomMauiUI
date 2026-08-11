using MauiUiSettings;

namespace MauiUiComponents;

public class ToggleGroupStore
{
    private readonly ComponentStore _componentStore;

    public ToggleGroupStyleStore Styles { get; }



    public ToggleGroupStore(ComponentStore componentStore)
    {
        _componentStore = componentStore;

        Styles = new ToggleGroupStyleStore(_componentStore);
    }


    public ToggleGroup<TItem, TLayout> ToggleGroup<TItem, TLayout>(
        IEnumerable<TItem> items,
        Func<TItem, IToggleItem> itemTemplate,
        TItem? selectedItem = default)
        where TLayout : Layout, new()
        where TItem : notnull
    {
        var toggleGroup = new ToggleGroup<TItem, TLayout>(
            _componentStore,
            itemTemplate)
        {
            SelectedItem = selectedItem,
            ItemsSource = items.ToList()
        };

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
            .TextStyleBind(_componentStore.UiServices, FontVariant.Text)
            .TextBind(
                localizationManager,
                localizationKey);

        toggleView.AddAction(actions);

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
            .TextStyleBind(_componentStore.UiServices, FontVariant.Icon)
            .TextIconBind(
                _componentStore,
                iconKey);

        toggleIconView.AddAction(actions);

        return toggleIconView;
    }
}
