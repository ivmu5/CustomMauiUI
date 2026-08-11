using MauiUiSettings;
using MauiUiSettings.Resources.Localization.Enum;

namespace MauiUiComponents;

public class SettingsPage : BasePage<ScrollView>
{
    private readonly VerticalStackLayout _layout;

    private readonly BaseBorder<ToggleGroup<ThemeType, FlexLayout>> _themeToggleBorder;
    private readonly BaseBorder<CornerRadiusSlider> _cornerRadiusSliderBorder;
    private readonly CustomDropdown<SupportedLanguage> _languageDropdown;

    private readonly BaseBorder<BaseButton> _saveButtonBorder;



    public SettingsPage(ComponentStore componentStore)
        : base(componentStore)
    {
        _themeToggleBorder = componentStore.Settings.ThemeToggle<BaseButton>().WithBorder(componentStore);
        _cornerRadiusSliderBorder = componentStore.Settings.CornerRadiusSlider().WithBorder(componentStore);
        _languageDropdown = componentStore.Settings.LangugaeDropdown(OverlayService);

        _saveButtonBorder = componentStore.Base
            .Button(ColorVariant.Primary)
            .TextBind(
                _componentStore.LocalizationStore.SettingsLocalization,
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

        HostLayout.Content = _layout;
    }

    private async void OnSaveButtonClicked(object? sender, EventArgs e)
    {
        await _componentStore.UiServices.UISettings.SaveAsync();
        _componentStore.Snackbar.Success(
            _componentStore.LocalizationStore.SettingsLocalization[nameof(UiSettingsResources.SettingsSaved)]);
    }
}