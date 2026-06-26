using MauiUiComponents.Resources.Localization;
using MauiUiSettings;
using SQLiteStorage;

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

        Value = _services.CornerRadiusService.CornerRadius;
        ValueChanged += OnValueChanged;

        _services.CornerRadiusService.PropertyChanged += OnCornerRadiusChanged;

        AddResetValueButton(CornerRadiusService.DefaultCornerRadius);

        TextLabel.TextBind(
            ComponentStore.ResourcesStore.SettingsLocalization,
            nameof(Settings.CornerRadiusSetting));
    }

    private void OnValueChanged(object? sender, ValueChangedEventArgs<int> e)
    {
        _services.CornerRadiusService.SetCornerRadius(e.NewValue);
    }

    private void OnCornerRadiusChanged(object? sender, EventArgs e)
    {
        var cornerRadius = _services.CornerRadiusService.CornerRadius;
        if (Value != cornerRadius)
            Value = cornerRadius;
    }
}