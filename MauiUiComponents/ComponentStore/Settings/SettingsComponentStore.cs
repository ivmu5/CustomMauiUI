using MauiUiSettings;
using MauiUiSettings.Resources.Localization.Enum;

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

    public CustomDropdown<SupportedLanguage> LangugaeDropdown(
        IOverlayService overlayService)
    {
        var dropdown = EnumView<SupportedLanguage>.Dropdown(
            overlayService,
            _componentStore,
            (item) =>
            {
                var toggleGrid = EnumView<SupportedLanguage>.CreateDefaultToggle(item, _componentStore);
                toggleGrid.AddAction(new ToggleBehavior<BaseGrid>(
                    toggleGrid.View,
                    _ => { _uiServices.LanguageService.SetLanguage(item); },
                    ToggleTrigger.BusinessAction));
                return toggleGrid;
            });


        dropdown.TextLabel.TextBind(
            _componentStore.ResourcesStore.SettingsLocalization,
            nameof(UiSettingsResources.SupportedLanguageSetting));

        dropdown.SelectedItem = _uiServices.LanguageService.Language;
        dropdown.Bind(
            dd => dd.SelectedItem,
            _uiServices.LanguageService,
            s => s.Language,
            BindingMode.TwoWay);
        dropdown.UpdateSelectedItemContent();

        return dropdown;
    }
}
