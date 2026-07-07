namespace MauiUiComponents;

public static class MauiUiComponentsCollectionExtensions
{
    public static IServiceCollection AddMauiUiComponents(
        this IServiceCollection services)
    {
        services.AddSingleton<ComponentStore>();
        services.AddSingleton<ResourcesStore>();
        services.AddSingleton<SettingsPage>();

        return services;
    }
}
