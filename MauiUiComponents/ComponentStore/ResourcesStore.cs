using MauiUiSettings.Resources.Localization.Enum;
using MauiUiSettings.Resources.Localization.MaterialSymbols;

namespace MauiUiComponents;

public class ResourcesStore
{
    public LocalizationResourceManager<UiSettingsResources> SettingsLocalization { get; init; }
    public LocalizationResourceManager<MaterialSymbols> MaterialSymbolsManager { get; init; }

    public ResourcesStore(
        LocalizationResourceManager<UiSettingsResources> settingsLocalization,
        LocalizationResourceManager<MaterialSymbols> materialSymbolsManager)
    {
        SettingsLocalization = settingsLocalization;
        MaterialSymbolsManager = materialSymbolsManager;
    }

    public ILocalizationResourceManager GetLocalizationManager(Type type)
    {
        if (type == typeof(UiSettingsResources))
            return SettingsLocalization;
        if (type == typeof(MaterialSymbols))
            return MaterialSymbolsManager;
        throw new NotSupportedException($"No localization manager found for enum type {type.Name}");
    }
}
