using MauiUiSettings.Resources.Localization.Enum;
using System.ComponentModel.DataAnnotations;

namespace MauiUiSettings;

public enum ThemeType
{
    [Display(
        Name = nameof(UiSettingsResources.ThemeSystem),
        ResourceType = typeof(UiSettingsResources))]
    System = AppTheme.Unspecified,

    [Display(
        Name = nameof(UiSettingsResources.ThemeLight),
        ResourceType = typeof(UiSettingsResources))]
    Light = AppTheme.Light,

    [Display(
        Name = nameof(UiSettingsResources.ThemeDark),
        ResourceType = typeof(UiSettingsResources))]
    Dark = AppTheme.Dark,
}
