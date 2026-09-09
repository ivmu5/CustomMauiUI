using MauiUiSettings;

namespace MauiUiComponents;

/// <summary>
/// Центральное хранилище UI-сервисов и фабрик компонентов
/// библиотеки MauiUiComponents.
/// </summary>
/// <remarks>
/// Используется как единая точка доступа к базовым,
/// пользовательским и специализированным компонентам,
/// а также к связанным UI-сервисам.
/// </remarks>
public sealed class ComponentStore
{
    #region Properties

    /// <summary>
    /// Набор глобальных UI-сервисов приложения.
    /// </summary>
    public UiServiceStore UiServices { get; }

    /// <summary>
    /// Хранилище ресурсов локализации.
    /// </summary>
    public LocalizationStore LocalizationStore { get; }

    /// <summary>
    /// Фабрика базовых UI-компонентов.
    /// </summary>
    public BaseComponentStore Base { get; }

    /// <summary>
    /// Фабрика пользовательских UI-компонентов.
    /// </summary>
    public CustomComponentStore Custom { get; }

    /// <summary>
    /// Фабрика компонентов,
    /// используемых на странице настроек.
    /// </summary>
    public SettingsComponentStore Settings { get; }

    /// <summary>
    /// Сервис отображения Snackbar-уведомлений.
    /// </summary>
    public SnackbarService Snackbar { get; }

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт центральное хранилище компонентов
    /// и инициализирует связанные фабрики.
    /// </summary>
    /// <param name="uiServices">
    /// Набор глобальных UI-сервисов.
    /// </param>
    /// <param name="localizationStore">
    /// Хранилище ресурсов локализации.
    /// </param>
    /// <param name="snackbar">
    /// Сервис отображения Snackbar-уведомлений.
    /// </param>
    public ComponentStore(
        UiServiceStore uiServices,
        LocalizationStore localizationStore,
        SnackbarService snackbar)
    {
        ArgumentNullException.ThrowIfNull(uiServices);
        ArgumentNullException.ThrowIfNull(localizationStore);
        ArgumentNullException.ThrowIfNull(snackbar);

        UiServices =
            uiServices;

        LocalizationStore =
            localizationStore;

        Snackbar =
            snackbar;

        Base =
            new BaseComponentStore(
                this);

        Custom =
            new CustomComponentStore(
                this);

        Settings =
            new SettingsComponentStore(
                this);
    }

    #endregion
}