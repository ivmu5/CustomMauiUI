using MauiUiSettings;

namespace MauiUiComponents;

public class SettingsComponentStore
{
    private readonly UiServiceStore _uiServices;
    private readonly ComponentStore _componentStore;



    public SettingsComponentStore(
        UiServiceStore uiServices,
        ComponentStore componentStore)
    {
        _uiServices = uiServices;
        _componentStore = componentStore;
    }

    public SettingsPage SettingsPage()
    {
        return new SettingsPage(_uiServices, _componentStore);
    }

    public ThemeToggle<TView> ThemeToggle<TView>()
        where TView : View, ITextComponent, new()
    {
        return new ThemeToggle<TView>(_uiServices, _componentStore);
    }

    public CornerRadiusSlider CornerRadiusSlider()
    {
        return new CornerRadiusSlider(_uiServices, _componentStore);
    }
}
