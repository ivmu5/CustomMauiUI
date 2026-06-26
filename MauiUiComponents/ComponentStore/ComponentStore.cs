using MauiUiSettings;

namespace MauiUiComponents;

public class ComponentStore
{
    public ResourcesStore ResourcesStore { get; init; }

    public BaseComponentStore Base { get; init; }
    public CustomComponentStore Custom { get; init; }
    public SettingsComponentStore Settings { get; init; }

    public SnackbarService Snackbar { get; init; }



    public ComponentStore(
        UiServiceStore uiServices,
        SnackbarService snackbarService,
        ResourcesStore resourcesStore)
    {
        ResourcesStore = resourcesStore;

        Base = new BaseComponentStore(uiServices);
        Custom = new CustomComponentStore(uiServices, this);
        Settings = new SettingsComponentStore(uiServices, this);
        Snackbar = snackbarService;
    }

}