using MauiUiSettings;
using MauiUiSettings.Resources.Localization.Enum;

namespace MauiUiComponents;

public class SettingsPage : BasePage<ScrollView>
{
    private readonly VerticalStackLayout _layout;

    private readonly BaseBorder<ThemeToggle<BaseButton>> _themeToggleBorder;
    private readonly BaseBorder<CornerRadiusSlider> _cornerRadiusSliderBorder;
    private readonly CustomDropdown<SupportedLanguage> _languageDropdown;

    private readonly BaseBorder<BaseButton> _saveButtonBorder;



    public SettingsPage(
        UiServiceStore uiServices,
        ComponentStore componentStore)
        : base(uiServices, componentStore)
    {
        _themeToggleBorder = componentStore.Settings.ThemeToggle<BaseButton>().WithBorder(componentStore);
        _cornerRadiusSliderBorder = componentStore.Settings.CornerRadiusSlider().WithBorder(componentStore);
        _languageDropdown = componentStore.Settings.LangugaeDropdown(OverlayService);

        _saveButtonBorder = componentStore.Base
            .Button(ColorVariant.Primary)
            .TextBind(
                _componentStore.ResourcesStore.SettingsLocalization,
                nameof(UiSettingsResources.SettingsSave))
            .WithBorder(componentStore);
        _saveButtonBorder.View.Clicked += OnSaveButtonClicked;

        _layout = new VerticalStackLayout()
        {
            Spacing = 10,
            Children =
            {
                _themeToggleBorder,
                _cornerRadiusSliderBorder,
                _languageDropdown,
                _saveButtonBorder
            }
        };

        AddChildren(_layout);
    }

    private async void OnSaveButtonClicked(object? sender, EventArgs e)
    {
        await _uiServices.UISettings.SaveAsync();
        _componentStore.Snackbar.Success(
            _componentStore.ResourcesStore.SettingsLocalization[nameof(UiSettingsResources.SettingsSaved)]);
    }
}