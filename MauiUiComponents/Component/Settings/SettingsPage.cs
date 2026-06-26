using MauiUiSettings;
using MauiUiComponents.Resources.Localization;

namespace MauiUiComponents;

public class SettingsPage : BasePage<ScrollView>
{
    private readonly VerticalStackLayout _layout;

    private readonly BaseBorder<ThemeToggle<BaseButton>> _themeToggleBorder;
    private readonly BaseBorder<CornerRadiusSlider> _cornerRadiusSliderBorder;

    private readonly BaseBorder<BaseButton> _saveButtonBorder;



    public SettingsPage(
        UiServiceStore uiServices,
        ComponentStore componentStore)
        : base(uiServices, componentStore)
    {
        _themeToggleBorder = componentStore.Settings.ThemeToggle<BaseButton>().WithBorder(componentStore);
        _cornerRadiusSliderBorder = componentStore.Settings.CornerRadiusSlider().WithBorder(componentStore);

        _saveButtonBorder = componentStore.Base
            .Button(ColorVariant.Primary)
            .TextBind(
                _componentStore.ResourcesStore.SettingsLocalization, 
                nameof(Settings.Save))
            .WithBorder(componentStore);
        _saveButtonBorder.View.Clicked += OnSaveButtonClicked;

        _layout = new VerticalStackLayout()
        {
            Spacing = 10,
            Children =
            {
                _themeToggleBorder,
                _cornerRadiusSliderBorder,
                _saveButtonBorder
            }
        };

        AddChildren(_layout);
    }

    private async void OnSaveButtonClicked(object? sender, EventArgs e)
    {
        await _uiServices.UISettings.SaveAsync();
        _componentStore.Snackbar.Success(
            _componentStore.ResourcesStore.SettingsLocalization[nameof(Settings.SettingsSaved)]);
    }
}