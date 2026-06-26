using MauiUiSettings.Resources.Localization.Enum;
using System.ComponentModel.DataAnnotations;

namespace MauiUiSettings;

public enum MaterialSymbolMode
{
    [Display(
        Name = nameof(EnumResources.MaterialSymbolModeAuto),
        ResourceType = typeof(EnumResources))]
    Auto,

    [Display(
        Name = nameof(EnumResources.MaterialSymbolModeRounded),
        ResourceType = typeof(EnumResources))]
    Rounded,

    [Display(
        Name = nameof(EnumResources.MaterialSymbolModeOutlined),
        ResourceType = typeof(EnumResources))]
    Outlined,

    [Display(
        Name = nameof(EnumResources.MaterialSymbolModeSharp),
        ResourceType = typeof(EnumResources))]
    Sharp
}
