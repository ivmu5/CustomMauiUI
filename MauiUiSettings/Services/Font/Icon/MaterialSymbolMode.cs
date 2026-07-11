using MauiUiSettings.Resources.Localization.Enum;
using System.ComponentModel.DataAnnotations;

namespace MauiUiSettings;

public enum MaterialSymbolMode
{
    [Display(
        Name = nameof(UiSettingsResources.MaterialSymbolModeAuto),
        ResourceType = typeof(UiSettingsResources))]
    Auto,

    [Display(
        Name = nameof(UiSettingsResources.MaterialSymbolModeRounded),
        ResourceType = typeof(UiSettingsResources))]
    Rounded,

    [Display(
        Name = nameof(UiSettingsResources.MaterialSymbolModeOutlined),
        ResourceType = typeof(UiSettingsResources))]
    Outlined,

    [Display(
        Name = nameof(UiSettingsResources.MaterialSymbolModeSharp),
        ResourceType = typeof(UiSettingsResources))]
    Sharp
}
