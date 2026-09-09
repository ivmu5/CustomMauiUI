using MauiUiSettings.Resources.Localization.Enum;
using MauiUiSettings.Resources.Localization.MaterialSymbols;

namespace MauiUiComponents;

/// <summary>
/// Хранит менеджеры локализации, используемые UI-компонентами,
/// и предоставляет доступ к ним по типу ресурса.
/// </summary>
public sealed class LocalizationStore
{
    #region Fields

    private readonly IReadOnlyDictionary<
        Type,
        ILocalizationResourceManager> _managers;

    #endregion

    #region Properties

    /// <summary>
    /// Менеджер локализации ресурсов пользовательских настроек.
    /// </summary>
    public LocalizationResourceManager<UiSettingsResources>
        SettingsLocalization
    { get; }

    /// <summary>
    /// Менеджер локализации символов Material Symbols.
    /// </summary>
    public LocalizationResourceManager<MaterialSymbols>
        MaterialSymbolsManager
    { get; }

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт хранилище доступных менеджеров локализации.
    /// </summary>
    /// <param name="settingsLocalization">
    /// Менеджер локализации ресурсов настроек UI.
    /// </param>
    /// <param name="materialSymbolsManager">
    /// Менеджер ресурсов Material Symbols.
    /// </param>
    public LocalizationStore(
        LocalizationResourceManager<UiSettingsResources> settingsLocalization,
        LocalizationResourceManager<MaterialSymbols> materialSymbolsManager)
    {
        ArgumentNullException.ThrowIfNull(settingsLocalization);
        ArgumentNullException.ThrowIfNull(materialSymbolsManager);

        SettingsLocalization =
            settingsLocalization;

        MaterialSymbolsManager =
            materialSymbolsManager;

        _managers =
            new Dictionary<Type, ILocalizationResourceManager>
            {
                [typeof(UiSettingsResources)] =
                    SettingsLocalization,

                [typeof(MaterialSymbols)] =
                    MaterialSymbolsManager
            };
    }

    #endregion

    #region Localization Managers

    /// <summary>
    /// Возвращает менеджер локализации,
    /// соответствующий указанному типу ресурса.
    /// </summary>
    /// <param name="type">
    /// Тип ресурса, для которого требуется менеджер локализации.
    /// </param>
    /// <returns>
    /// Зарегистрированный менеджер локализации.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Для указанного типа ресурса менеджер не зарегистрирован.
    /// </exception>
    public ILocalizationResourceManager GetLocalizationManager(
        Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (_managers.TryGetValue(
                type,
                out var manager))
        {
            return manager;
        }

        throw new NotSupportedException(
            $"Менеджер локализации для типа '{type.Name}' не зарегистрирован.");
    }

    #endregion
}