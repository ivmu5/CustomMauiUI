using MauiUiSettings.Resources.Localization.Enum;
using System.ComponentModel.DataAnnotations;

namespace MauiUiSettings;

public enum SupportedLanguage
{
    [Display(
        Name = nameof(UiSettingsResources.SupportedLanguageEnglish),
        ResourceType = typeof(UiSettingsResources))]
    [LanguageCode("en")]
    English,


    [Display(
        Name = nameof(UiSettingsResources.SupportedLanguageRussian),
        ResourceType = typeof(UiSettingsResources))]
    [LanguageCode("ru")]
    Russian
}
