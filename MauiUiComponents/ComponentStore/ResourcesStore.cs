using MauiUiSettings.Resources.Localization.MaterialSymbols;

namespace MauiUiComponents;

public class ResourcesStore
{
    public LocalizationResourceManager<Resources.Localization.Settings> SettingsLocalization { get; init; }
    public LocalizationResourceManager<MaterialSymbols> MaterialSymbolsManager { get; init; }

    public ResourcesStore(
        LocalizationResourceManager<Resources.Localization.Settings> settingsLocalization,
        LocalizationResourceManager<MaterialSymbols> materialSymbolsManager)
    {
        SettingsLocalization = settingsLocalization;
        MaterialSymbolsManager = materialSymbolsManager;
    }
}
