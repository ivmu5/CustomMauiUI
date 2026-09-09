using MauiUiSettings;
using MauiUiSettings.Resources.Localization.Enum;
using SQLiteStorage;
using System.ComponentModel;

namespace MauiUiComponents;

/// <summary>
/// Предоставляет фабричные методы для создания
/// UI-компонентов страницы настроек.
/// </summary>
public sealed class SettingsComponentStore
{
    #region Fields

    private readonly ComponentStore _componentStore;

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт фабрику компонентов страницы настроек.
    /// </summary>
    /// <param name="componentStore">
    /// Центральное хранилище UI-компонентов и сервисов.
    /// </param>
    public SettingsComponentStore(
        ComponentStore componentStore)
    {
        ArgumentNullException.ThrowIfNull(
            componentStore);

        _componentStore =
            componentStore;
    }

    #endregion

    #region Settings Page

    /// <summary>
    /// Создаёт новое представление настроек.
    /// </summary>
    /// <returns>
    /// Новый экземпляр <see cref="SettingsPage"/>.
    /// </returns>
    public SettingsPage SettingsPage()
    {
        return new SettingsPage(
            _componentStore);
    }

    #endregion

    #region Theme

    /// <summary>
    /// Создаёт toggle-группу для выбора темы приложения.
    /// </summary>
    /// <typeparam name="TView">
    /// Тип текстового представления.
    /// Параметр сохранён для совместимости
    /// с текущим публичным API.
    /// </typeparam>
    /// <returns>
    /// Настроенная группа выбора темы.
    /// </returns>
    public ToggleGroup<ThemeType, FlexLayout> ThemeToggle<TView>()
        where TView : View, ITextComponent, new()
    {
        var themeToggle =
            EnumView<ThemeType>.ToggleGroup<FlexLayout>(
                _componentStore,
                theme =>
                    EnumView<ThemeType>.CreateDefaultToggle(
                        theme,
                        _componentStore,
                        () => _componentStore.Base.Button()));

        themeToggle.UseCaptionLabel =
            true;

        themeToggle.CaptionLabel.TextBind(
            _componentStore.LocalizationStore.SettingsLocalization,
            nameof(UiSettingsResources.ThemeSetting));

        themeToggle.SelectedItem =
            _componentStore.UiServices
                .ThemeService
                .CurrentTheme;

        themeToggle.SelectionChanged +=
            OnThemeSelectionChanged;

        /*
         * Сервис используется как источник состояния UI.
         * Пользовательский выбор передаётся сервису явно
         * через SelectionChanged.
         */
        themeToggle.Bind(
            target => target.SelectedItem,
            _componentStore.UiServices.ThemeService,
            source => source.CurrentTheme,
            BindingMode.OneWay);

        themeToggle.ToggleLayout
            .FlexEqualGrow();

        return themeToggle;
    }

    /// <summary>
    /// Применяет тему, выбранную пользователем
    /// в соответствующей toggle-группе.
    /// </summary>
    private void OnThemeSelectionChanged(
        object? sender,
        ValueChangedEventArgs<ThemeType> e)
    {
        if (sender is not
            ToggleGroup<ThemeType, FlexLayout> themeToggle)
        {
            return;
        }

        var theme =
            themeToggle.SelectedItem;

        var themeService =
            _componentStore.UiServices.ThemeService;

        if (EqualityComparer<ThemeType>.Default.Equals(
                themeService.CurrentTheme,
                theme))
        {
            return;
        }

        themeService.SetTheme(
            theme);
    }

    #endregion

    #region Corner Radius

    /// <summary>
    /// Создаёт числовой слайдер для изменения
    /// глобального радиуса скругления UI-компонентов.
    /// </summary>
    /// <returns>
    /// Настроенный компонент управления радиусом скругления.
    /// </returns>
    public CustomTextSlider<int> CornerRadiusSlider()
    {
        var cornerRadiusService =
            _componentStore.UiServices.CornerRadiusService;

        var slider =
            _componentStore.Custom
                .TextSlider<int>();

        slider.SetRange(
            CornerRadiusService.MinCornerRadius,
            CornerRadiusService.MaxCornerRadius);

        slider.Step =
            1;

        slider.DefaultValue =
            CornerRadiusService.DefaultCornerRadius;

        slider.BindableValue =
            cornerRadiusService.CornerRadius;

        /*
         * Сервис является источником состояния.
         *
         * TwoWay здесь намеренно не используется:
         * непосредственная запись Binding в CornerRadius
         * обошла бы SetCornerRadius(), а значит не обновила бы
         * RoundRectangle и сохранённое значение UISettings.
         */
        slider.Bind(
            target => target.BindableValue,
            cornerRadiusService,
            source => source.CornerRadius,
            BindingMode.OneWay);

        slider.PropertyChanged +=
            OnCornerRadiusSliderPropertyChanged;

        return slider;
    }

    /// <summary>
    /// Передаёт изменение радиуса скругления
    /// из UI в <see cref="CornerRadiusService"/>.
    /// </summary>
    private void OnCornerRadiusSliderPropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        if (e.PropertyName !=
            nameof(CustomTextSlider<int>.BindableValue))
        {
            return;
        }

        if (sender is not
            CustomTextSlider<int> slider)
        {
            return;
        }

        var cornerRadiusService =
            _componentStore.UiServices.CornerRadiusService;

        var newValue =
            slider.BindableValue;

        if (cornerRadiusService.CornerRadius ==
            newValue)
        {
            return;
        }

        /*
         * Используем публичный метод сервиса,
         * чтобы одновременно обновились:
         *
         * CornerRadius,
         * RoundRectangle,
         * UISettings.CornerRadius.
         */
        cornerRadiusService.SetCornerRadius(
            newValue);
    }

    #endregion

    #region Language

    /// <summary>
    /// Создаёт dropdown для выбора языка приложения.
    /// </summary>
    /// <param name="overlayService">
    /// Сервис отображения выпадающего списка.
    /// </param>
    /// <returns>
    /// Настроенный dropdown выбора языка.
    /// </returns>
    public CustomDropdown<SupportedLanguage> LanguageDropdown(
        IOverlayService overlayService)
    {
        ArgumentNullException.ThrowIfNull(
            overlayService);

        var dropdown =
            EnumView<SupportedLanguage>.Dropdown(
                overlayService,
                _componentStore,
                CreateLanguageToggle);

        dropdown.UseCaptionLabel =
            true;

        dropdown.CaptionLabel.TextBind(
            _componentStore.LocalizationStore.SettingsLocalization,
            nameof(UiSettingsResources.SupportedLanguageSetting));

        dropdown.SelectedItem =
            _componentStore.UiServices
                .LanguageService
                .Language;

        dropdown.SelectionChanged +=
            OnLanguageSelectionChanged;

        /*
         * Изменения LanguageService автоматически отражаются
         * в dropdown, а пользовательский выбор передаётся
         * сервису через SelectionChanged.
         */
        dropdown.Bind(
            target => target.SelectedItem,
            _componentStore.UiServices.LanguageService,
            source => source.Language,
            BindingMode.OneWay);

        return dropdown;
    }

    /// <summary>
    /// Создаёт визуальное представление
    /// одного языка внутри dropdown.
    /// </summary>
    private IToggleItem CreateLanguageToggle(
        SupportedLanguage language)
    {
        var toggle =
            EnumView<SupportedLanguage>.CreateDefaultToggle(
                language,
                _componentStore,
                () => _componentStore.Base.Label());

        toggle.View
            .TextLeft()
            .ViewVerticalCenter();

        toggle.View.Padding =
            10;

        return toggle;
    }

    /// <summary>
    /// Применяет выбранный язык и показывает
    /// возможность отменить изменение через Snackbar.
    /// </summary>
    private void OnLanguageSelectionChanged(
        object? sender,
        ValueChangedEventArgs<SupportedLanguage> e)
    {
        if (sender is not
            CustomDropdown<SupportedLanguage> dropdown)
        {
            return;
        }

        var languageService =
            _componentStore.UiServices.LanguageService;

        var newLanguage =
            dropdown.SelectedItem;

        var oldLanguage =
            languageService.Language;

        if (EqualityComparer<SupportedLanguage>.Default.Equals(
                oldLanguage,
                newLanguage))
        {
            return;
        }

        languageService.SetLanguage(
            newLanguage);

        _componentStore.Snackbar.Show(
            new SnackbarMessage
            {
                ActionText =
                    UiSettingsResources.SettingsCancel,

                Action = () =>
                {
                    languageService.SetLanguage(
                        oldLanguage);

                    return Task.CompletedTask;
                },

                IsPriority =
                    true
            });
    }

    #endregion
}