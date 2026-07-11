using MauiUiSettings;
using MauiUiSettings.Resources.Localization.Enum;
using System.ComponentModel;

namespace MauiUiComponents;

public class CornerRadiusSlider : CustomTextSlider<int>
{
    private readonly UiServiceStore _services;

    public CornerRadiusSlider(
        UiServiceStore services,
        ComponentStore ComponentStore)
        : base(ComponentStore)
    {
        _services = services;

        SetRange(
            CornerRadiusService.MinCornerRadius,
            CornerRadiusService.MaxCornerRadius);

        BindableValue = _services.CornerRadiusService.CornerRadius;
        PropertyChanged += OnPropertyChanged;
        _services.CornerRadiusService.PropertyChanged += OnCornerRadiusChanged;

        AddResetValueButton(CornerRadiusService.DefaultCornerRadius);

        TextLabel.TextBind(
            ComponentStore.ResourcesStore.SettingsLocalization,
            nameof(UiSettingsResources.CornerRadiusSetting));
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(BindableValue))
            _services.CornerRadiusService.SetCornerRadius(BindableValue);
    }

    private void OnCornerRadiusChanged(object? sender, EventArgs e)
    {
        var cornerRadius = _services.CornerRadiusService.CornerRadius;
        if (BindableValue != cornerRadius)
            BindableValue = cornerRadius;
    }
}