using MauiUiSettings;

namespace MauiUiComponents;

public class ComponentStore
{
    public UiServiceStore UiServices { get; }

    public LocalizationStore LocalizationStore { get; }

    public BaseComponentStore Base { get; }
    public CustomComponentStore Custom { get; }
    public SettingsComponentStore Settings { get; }

    public SnackbarService Snackbar { get; }



    public ComponentStore(
        UiServiceStore uiServices,
        SnackbarService snackbarService,
        LocalizationStore localizationStore)
    {
        LocalizationStore = localizationStore;
        UiServices = uiServices;

        Base = new BaseComponentStore(this);
        Custom = new CustomComponentStore(this);
        Settings = new SettingsComponentStore(this);
        Snackbar = snackbarService;
    }

}