using Microsoft.Extensions.DependencyInjection;

namespace MauiUiComponents;

/// <summary>
/// Содержит методы расширения для регистрации
/// компонентов MauiUiComponents в контейнере зависимостей.
/// </summary>
public static class MauiUiComponentsCollectionExtensions
{
    #region Registration

    /// <summary>
    /// Регистрирует основные сервисы, хранилища,
    /// навигацию и представления MauiUiComponents.
    /// </summary>
    /// <param name="services">
    /// Коллекция сервисов приложения.
    /// </param>
    /// <returns>
    /// Исходная коллекция сервисов для продолжения
    /// fluent-конфигурации зависимостей.
    /// </returns>
    public static IServiceCollection AddMauiUiComponents(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        RegisterStores(
            services);

        RegisterNavigation(
            services);

        RegisterViews(
            services);

        return services;
    }

    #endregion

    #region Stores

    /// <summary>
    /// Регистрирует центральные хранилища
    /// и фабрики UI-компонентов.
    /// </summary>
    private static void RegisterStores(
        IServiceCollection services)
    {
        services.AddSingleton<LocalizationStore>();
        services.AddSingleton<ComponentStore>();
    }

    #endregion

    #region Navigation

    /// <summary>
    /// Регистрирует глобальную навигационную инфраструктуру приложения.
    /// </summary>
    private static void RegisterNavigation(
        IServiceCollection services)
    {
        /*
         * NavigationHostPage существует в единственном экземпляре,
         * поскольку является физическим корнем UI приложения.
         */
        services.AddSingleton<NavigationHostPage>();

        /*
         * NavigationService хранит глобальную историю переходов,
         * поэтому его жизненный цикл также соответствует приложению.
         */
        services.AddSingleton<NavigationService>();
    }

    #endregion

    #region Views

    /// <summary>
    /// Регистрирует стандартные представления,
    /// предоставляемые библиотекой MauiUiComponents.
    /// </summary>
    private static void RegisterViews(
        IServiceCollection services)
    {
        services.AddSingleton<SettingsPage>();
    }

    #endregion
}