using MauiUiSettings.Resources.Localization.Enum;
using System.ComponentModel.DataAnnotations;

namespace MauiUiSettings;

public enum SupportedLanguage
{
    [Display(
        Name = nameof(EnumResources.SupportedLanguageEnglish),
        ResourceType = typeof(EnumResources))]
    [LanguageCode("en")]
    English,



    [Display(
        Name = nameof(EnumResources.SupportedLanguageRussian),
        ResourceType = typeof(EnumResources))]
    [LanguageCode("ru")]
    Russian
}
