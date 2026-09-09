using MauiUiSettings;

namespace MauiUiComponents;

/// <summary>
/// Содержит методы расширения для настройки и привязки
/// текстовых свойств компонентов, реализующих <see cref="ITextComponent"/>.
/// </summary>
public static class TextComponentExtensions
{
    #region Style

    /// <summary>
    /// Применяет к текстовому компоненту стандартный стиль шрифта.
    /// </summary>
    /// <param name="view">
    /// Компонент, к которому применяется стиль.
    /// </param>
    /// <param name="uiServices">
    /// Набор UI-сервисов, содержащий сервисы цветов и шрифтов.
    /// </param>
    /// <param name="fontVariant">
    /// Тип текстового стиля: обычный текст или иконка.
    /// </param>
    /// <returns>
    /// Исходный компонент для продолжения fluent-цепочки.
    /// </returns>
    public static T TextStyleBind<T>(
        this T view,
        UiServiceStore uiServices,
        FontVariant fontVariant)
        where T : BindableObject, ITextComponent
    {
        ArgumentNullException.ThrowIfNull(view);
        ArgumentNullException.ThrowIfNull(uiServices);

        return fontVariant switch
        {
            FontVariant.Text =>
                view.ApplyTextStyle(
                    uiServices.ColorService,
                    uiServices.FontService),

            FontVariant.Icon =>
                view.ApplyTextStyle(
                    uiServices.ColorService,
                    uiServices.IconService),

            _ =>
                throw new ArgumentOutOfRangeException(
                    nameof(fontVariant),
                    fontVariant,
                    null)
        };
    }

    /// <summary>
    /// Применяет к компоненту цвет текста, размер шрифта
    /// и семейство шрифта из указанного сервиса.
    /// </summary>
    private static T ApplyTextStyle<T, TFontService>(
        this T view,
        ColorService colorService,
        BaseFontService<TFontService> fontService)
        where T : BindableObject, ITextComponent
    {
        return view
            .TextColorBind(colorService)
            .FontSizeBind(fontService)
            .FontFamilyBind(fontService);
    }

    #endregion

    #region Color

    /// <summary>
    /// Привязывает цвет текста компонента к текущему цвету текста
    /// из <see cref="ColorService"/>.
    /// </summary>
    /// <returns>
    /// Исходный компонент для продолжения fluent-цепочки.
    /// </returns>
    public static T TextColorBind<T>(
        this T view,
        ColorService colorService)
        where T : BindableObject, ITextComponent
    {
        ArgumentNullException.ThrowIfNull(view);
        ArgumentNullException.ThrowIfNull(colorService);

        return view.Bind(
            target => target.TextColor,
            colorService,
            service => service.Text);
    }

    #endregion

    #region Font

    /// <summary>
    /// Привязывает размер шрифта компонента
    /// к размеру из указанного сервиса шрифтов.
    /// </summary>
    /// <returns>
    /// Исходный компонент для продолжения fluent-цепочки.
    /// </returns>
    public static T FontSizeBind<T, TFontService>(
        this T view,
        BaseFontService<TFontService> fontService)
        where T : BindableObject, ITextComponent
    {
        ArgumentNullException.ThrowIfNull(view);
        ArgumentNullException.ThrowIfNull(fontService);

        return view.Bind(
            target => target.FontSize,
            fontService,
            service => service.FontSize);
    }

    /// <summary>
    /// Привязывает семейство шрифта компонента
    /// к семейству из указанного сервиса шрифтов.
    /// </summary>
    /// <returns>
    /// Исходный компонент для продолжения fluent-цепочки.
    /// </returns>
    public static T FontFamilyBind<T, TFontService>(
        this T view,
        BaseFontService<TFontService> fontService)
        where T : BindableObject, ITextComponent
    {
        ArgumentNullException.ThrowIfNull(view);
        ArgumentNullException.ThrowIfNull(fontService);

        return view.Bind(
            target => target.FontFamily,
            fontService,
            service => service.FontFamily);
    }

    #endregion

    #region Localization

    /// <summary>
    /// Привязывает текст компонента к локализованной иконке
    /// из набора Material Symbols.
    /// </summary>
    /// <param name="iconKey">
    /// Ключ иконки в менеджере локализации Material Symbols.
    /// </param>
    /// <returns>
    /// Исходный компонент для продолжения fluent-цепочки.
    /// </returns>
    public static T TextIconBind<T>(
        this T view,
        ComponentStore componentStore,
        string iconKey)
        where T : BindableObject, ITextComponent
    {
        ArgumentNullException.ThrowIfNull(view);
        ArgumentNullException.ThrowIfNull(componentStore);
        ArgumentException.ThrowIfNullOrWhiteSpace(iconKey);

        return view.TextBind(
            componentStore.LocalizationStore.MaterialSymbolsManager,
            iconKey);
    }

    /// <summary>
    /// Привязывает текст компонента к значению
    /// из указанного менеджера локализации.
    /// </summary>
    /// <param name="key">
    /// Ключ локализованного значения.
    /// </param>
    /// <returns>
    /// Исходный компонент для продолжения fluent-цепочки.
    /// </returns>
    public static T TextBind<T>(
        this T view,
        ILocalizationResourceManager localizationManager,
        string key)
        where T : BindableObject, ITextComponent
    {
        ArgumentNullException.ThrowIfNull(view);
        ArgumentNullException.ThrowIfNull(localizationManager);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        var textProperty =
            BindingExtensions.GetBindableProperty<T, string>(
                target => target.Text);

        /*
         * Локализационный менеджер предоставляет значения через индексатор.
         * Путь вида "[Key]" позволяет MAUI Binding автоматически обновлять
         * текст при изменении текущего языка.
         */
        view.SetBinding(
            textProperty,
            new Binding(
                $"[{key}]",
                source: localizationManager));

        return view;
    }

    #endregion
}