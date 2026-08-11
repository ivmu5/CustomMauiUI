using MauiUiSettings;
using MauiUiSettings.Resources.Localization.Enum;

namespace MauiUiComponents;

public class SettingsComponentStore
{
    private readonly ComponentStore _componentStore;

    public SettingsComponentStore(ComponentStore componentStore)
    {
        _componentStore = componentStore;
    }

    public SettingsPage SettingsPage()
    {
        return new SettingsPage(_componentStore);
    }

    public ToggleGroup<ThemeType, FlexLayout> ThemeToggle<TView>()
        where TView : View, ITextComponent, new()
    {
        var themeToggle = EnumView<ThemeType>.ToggleGroup<FlexLayout>(
            _componentStore,
            (theme) =>
            {
                var toggleBtn = EnumView<ThemeType>.CreateDefaultToggle(
                    theme, 
                    _componentStore,
                    () => _componentStore.Base.Button());
                toggleBtn.AddAction(new ToggleAction<BaseButton>(
                    toggleBtn.View,
                    "SetThemeAction",
                    _ => _componentStore.UiServices.ThemeService.SetTheme(theme),
                    ToggleActionTrigger.BusinessAction));

                return toggleBtn;
            });

        themeToggle.UseCaptionLabel = true;
        themeToggle.CaptionLabel.TextBind(
            _componentStore.LocalizationStore.SettingsLocalization,
            nameof(UiSettingsResources.ThemeSetting));

        themeToggle.SelectedItem = _componentStore.UiServices.ThemeService.CurrentTheme;
        themeToggle.Bind(
            tt => tt.SelectedItem,
            _componentStore.UiServices.ThemeService,
            s => s.CurrentTheme);

        themeToggle.ToggleLayout.FlexEqualGrow();

        return themeToggle;
    }

    public CornerRadiusSlider CornerRadiusSlider()
    {
        return new CornerRadiusSlider(_componentStore.UiServices, _componentStore);
    }

    public CustomDropdown<SupportedLanguage> LangugaeDropdown(
        IOverlayService overlayService)
    {
        var dropdown = EnumView<SupportedLanguage>.Dropdown(
            overlayService,
            _componentStore,
            (item) =>
            {
                var toggleLabel = EnumView<SupportedLanguage>.CreateDefaultToggle(
                    item,
                    _componentStore,
                    () => _componentStore.Base.Label());

                toggleLabel.View
                    .TextLeft()
                    .ViewVerticalCenter()
                    .Padding = 10;

                toggleLabel.AddAction(new ToggleAction<BaseLabel>(
                    toggleLabel.View,
                    "SetLanguageAction",
                    _ =>
                    {
                        _componentStore.Snackbar.Show(
                            new SnackbarMessage()
                            {
                                ActionText = UiSettingsResources.SettingsCancel,
                                Action = async () =>
                                {
                                    var curLang = _componentStore.UiServices.LanguageService.Language;
                                    _componentStore.UiServices.LanguageService.SetLanguage(curLang);
                                },
                                IsPriority = true

                            });
                        _componentStore.UiServices.LanguageService.SetLanguage(item);
                    },
                    ToggleActionTrigger.BusinessAction));
                return toggleLabel;
            });

        dropdown.UseCaptionLabel = true;
        dropdown.CaptionLabel.TextBind(
            _componentStore.LocalizationStore.SettingsLocalization,
            nameof(UiSettingsResources.SupportedLanguageSetting));

        dropdown.SelectedItem = _componentStore.UiServices.LanguageService.Language;
        dropdown.Bind(
            dd => dd.SelectedItem,
            _componentStore.UiServices.LanguageService,
            s => s.Language,
            BindingMode.TwoWay);
        //dropdown.UpdateSelectedItemContent();

        return dropdown;
    }
}
