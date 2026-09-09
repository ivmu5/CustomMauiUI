using MauiUiSettings;
using MauiUiSettings.Resources.Localization.Enum;

namespace MauiUiComponents;

/// <summary>
/// Представление пользовательских настроек приложения.
/// Содержит элементы выбора темы, радиуса скругления,
/// языка интерфейса и кнопку сохранения настроек.
/// </summary>
public sealed class SettingsPage :
    BasePage<ScrollView>,
    IDisposable
{
    #region Fields

    private readonly VerticalStackLayout _layout;

    private readonly BaseBorder<ToggleGroup<ThemeType, FlexLayout>>
        _themeToggleBorder;

    private readonly BaseBorder<CustomTextSlider<int>>
        _cornerRadiusSliderBorder;

    private readonly CustomDropdown<SupportedLanguage>
        _languageDropdown;

    private readonly BaseBorder<BaseButton>
        _saveButtonBorder;

    private bool _disposed;

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт представление настроек
    /// и формирует его пользовательский интерфейс.
    /// </summary>
    /// <param name="componentStore">
    /// Центральное хранилище UI-компонентов и сервисов.
    /// </param>
    public SettingsPage(
        ComponentStore componentStore)
        : base(componentStore)
    {
        ArgumentNullException.ThrowIfNull(
            componentStore);

        _themeToggleBorder =
            componentStore.Settings
                .ThemeToggle<BaseButton>()
                .WithBorder(
                    componentStore);

        _cornerRadiusSliderBorder =
            componentStore.Settings
                .CornerRadiusSlider()
                .WithBorder(
                    componentStore);

        _languageDropdown =
            componentStore.Settings
                .LanguageDropdown(
                    OverlayService);

        _saveButtonBorder =
            CreateSaveButton();

        _layout =
            new VerticalStackLayout
            {
                Spacing = 10,
                Padding = 10
            };

        BuildLayout();
    }

    #endregion

    #region Layout

    /// <summary>
    /// Формирует содержимое представления настроек.
    /// </summary>
    private void BuildLayout()
    {
        _layout.AddChildren(
            _themeToggleBorder,
            _cornerRadiusSliderBorder,
            _languageDropdown,
            _saveButtonBorder);

        HostLayout.Content =
            _layout;
    }

    #endregion

    #region Save

    /// <summary>
    /// Создаёт кнопку сохранения настроек
    /// и регистрирует обработчик её нажатия.
    /// </summary>
    private BaseBorder<BaseButton> CreateSaveButton()
    {
        var buttonBorder =
            _componentStore.Base
                .Button(
                    ColorVariant.Primary)
                .TextBind(
                    _componentStore.LocalizationStore.SettingsLocalization,
                    nameof(UiSettingsResources.SettingsSave))
                .WithBorder(
                    _componentStore);

        buttonBorder.View.Clicked +=
            OnSaveButtonClicked;

        return buttonBorder;
    }

    /// <summary>
    /// Обрабатывает нажатие кнопки сохранения настроек.
    /// </summary>
    private async void OnSaveButtonClicked(
        object? sender,
        EventArgs e)
    {
        await SaveSettingsAsync();
    }

    /// <summary>
    /// Сохраняет текущие UI-настройки
    /// и показывает уведомление об успешном сохранении.
    /// </summary>
    private async Task SaveSettingsAsync()
    {
        await _componentStore.UiServices
            .UISettings
            .SaveAsync();

        _componentStore.Snackbar.Success(
            _componentStore.LocalizationStore
                .SettingsLocalization[
                    nameof(UiSettingsResources.SettingsSaved)]);
    }

    #endregion

    #region Dispose

    /// <summary>
    /// Освобождает обработчики событий
    /// и ресурсы дочерних компонентов.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        _saveButtonBorder.View.Clicked -=
            OnSaveButtonClicked;

        _languageDropdown.Dispose();

        _cornerRadiusSliderBorder
            .View
            .Dispose();

        _disposed =
            true;

        GC.SuppressFinalize(
            this);
    }

    #endregion
}