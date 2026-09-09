using MauiUiSettings;
using System.Numerics;

namespace MauiUiComponents;

/// <summary>
/// Предоставляет фабричные методы для создания
/// базовых UI-компонентов библиотеки MauiUiComponents.
/// </summary>
public sealed class BaseComponentStore
{
    #region Fields

    private readonly ComponentStore _componentStore;

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт фабрику базовых UI-компонентов.
    /// </summary>
    /// <param name="componentStore">
    /// Центральное хранилище UI-компонентов и сервисов.
    /// </param>
    public BaseComponentStore(
        ComponentStore componentStore)
    {
        ArgumentNullException.ThrowIfNull(
            componentStore);

        _componentStore =
            componentStore;
    }

    #endregion

    #region Button

    /// <summary>
    /// Создаёт базовую кнопку и применяет
    /// стандартное оформление библиотеки.
    /// </summary>
    /// <param name="backgroundColor">
    /// Цвет фона кнопки.
    /// </param>
    /// <param name="fontVariant">
    /// Используемый тип шрифта.
    /// </param>
    /// <returns>
    /// Настроенная кнопка.
    /// </returns>
    public BaseButton Button(
        ColorVariant backgroundColor = ColorVariant.Secondary,
        FontVariant fontVariant = FontVariant.Text)
    {
        var button =
            new BaseButton();

        ApplyBaseStyle(
            button,
            backgroundColor,
            fontVariant);

        return button;
    }

    #endregion

    #region Label

    /// <summary>
    /// Создаёт базовую текстовую метку
    /// и применяет стандартное оформление текста.
    /// </summary>
    /// <param name="fontVariant">
    /// Используемый тип шрифта.
    /// </param>
    /// <returns>
    /// Настроенная текстовая метка.
    /// </returns>
    public BaseLabel Label(
        FontVariant fontVariant = FontVariant.Text)
    {
        var label =
            new BaseLabel();

        label.TextStyleBind(
            _componentStore.UiServices,
            fontVariant);

        return label;
    }

    #endregion

    #region Editor

    /// <summary>
    /// Создаёт базовый многострочный редактор
    /// и применяет стандартное оформление библиотеки.
    /// </summary>
    /// <param name="backgroundColor">
    /// Цвет фона редактора.
    /// </param>
    /// <param name="fontVariant">
    /// Используемый тип шрифта.
    /// </param>
    /// <returns>
    /// Настроенный редактор.
    /// </returns>
    public BaseEditor Editor(
        ColorVariant backgroundColor = ColorVariant.Secondary,
        FontVariant fontVariant = FontVariant.Text)
    {
        var editor =
            new BaseEditor();

        ApplyBaseStyle(
            editor,
            backgroundColor,
            fontVariant);

        return editor;
    }

    #endregion

    #region Entry

    /// <summary>
    /// Создаёт типизированное поле ввода
    /// и применяет стандартное оформление библиотеки.
    /// </summary>
    /// <typeparam name="TValue">
    /// Тип значения поля ввода.
    /// </typeparam>
    /// <param name="backgroundColor">
    /// Цвет фона поля ввода.
    /// </param>
    /// <param name="fontVariant">
    /// Используемый тип шрифта.
    /// </param>
    /// <returns>
    /// Настроенное типизированное поле ввода.
    /// </returns>
    public BaseEntry<TValue> Entry<TValue>(
        ColorVariant backgroundColor = ColorVariant.Secondary,
        FontVariant fontVariant = FontVariant.Text)
    {
        var entry =
            EntryFactory.Create<TValue>();

        ApplyBaseStyle(
            entry,
            backgroundColor,
            fontVariant);

        return entry;
    }

    #endregion

    #region Slider

    /// <summary>
    /// Создаёт типизированный слайдер
    /// и привязывает его цвета к глобальной палитре приложения.
    /// </summary>
    /// <typeparam name="TValue">
    /// Числовой тип значения слайдера.
    /// </typeparam>
    /// <param name="minimumTrackColor">
    /// Цвет заполненной части трека.
    /// </param>
    /// <param name="maximumTrackColor">
    /// Цвет незаполненной части трека.
    /// </param>
    /// <param name="thumbColor">
    /// Цвет ползунка.
    /// </param>
    /// <returns>
    /// Настроенный типизированный слайдер.
    /// </returns>
    public BaseSlider<TValue> Slider<TValue>(
        ColorVariant minimumTrackColor = ColorVariant.Primary,
        ColorVariant maximumTrackColor = ColorVariant.Secondary,
        ColorVariant thumbColor = ColorVariant.Primary)
        where TValue :
            INumber<TValue>,
            IMinMaxValue<TValue>
    {
        var slider =
            new BaseSlider<TValue>();

        slider.ColorBind(
            _componentStore.UiServices,
            target => target.MinimumTrackColor,
            minimumTrackColor);

        slider.ColorBind(
            _componentStore.UiServices,
            target => target.MaximumTrackColor,
            maximumTrackColor);

        slider.ColorBind(
            _componentStore.UiServices,
            target => target.ThumbColor,
            thumbColor);

        return slider;
    }

    #endregion

    #region Border

    /// <summary>
    /// Создаёт типизированную рамку вокруг указанного представления
    /// и привязывает её оформление к текущим UI-настройкам.
    /// </summary>
    /// <typeparam name="TView">
    /// Тип вложенного представления.
    /// </typeparam>
    /// <param name="view">
    /// Представление, помещаемое внутрь рамки.
    /// </param>
    /// <param name="strokeColor">
    /// Цвет обводки рамки.
    /// </param>
    /// <param name="backgroundColor">
    /// Цвет фона рамки.
    /// </param>
    /// <returns>
    /// Настроенная рамка.
    /// </returns>
    public BaseBorder<TView> Border<TView>(
        TView view,
        ColorVariant strokeColor = ColorVariant.Primary,
        ColorVariant backgroundColor = ColorVariant.None)
        where TView : View
    {
        ArgumentNullException.ThrowIfNull(
            view);

        var border =
            new BaseBorder<TView>(
                view);

        border.ColorBind(
            _componentStore.UiServices,
            target => target.Stroke,
            strokeColor);

        border.ColorBackgroundBind(
            _componentStore.UiServices,
            backgroundColor);

        border.BorderRoundRectangleBind(
            _componentStore.UiServices);

        return border;
    }

    #endregion

    #region Base Style

    /// <summary>
    /// Применяет стандартное оформление
    /// к текстовому UI-компоненту.
    /// </summary>
    /// <typeparam name="TView">
    /// Тип настраиваемого представления.
    /// </typeparam>
    /// <param name="view">
    /// Настраиваемое представление.
    /// </param>
    /// <param name="backgroundColor">
    /// Цвет фона.
    /// </param>
    /// <param name="fontVariant">
    /// Используемый тип шрифта.
    /// </param>
    /// <returns>
    /// Исходное представление.
    /// </returns>
    private TView ApplyBaseStyle<TView>(
        TView view,
        ColorVariant backgroundColor,
        FontVariant fontVariant)
        where TView : View, ITextComponent
    {
        ArgumentNullException.ThrowIfNull(
            view);

        view.ColorBackgroundBind(
            _componentStore.UiServices,
            backgroundColor);

        view.TextStyleBind(
            _componentStore.UiServices,
            fontVariant);

        return view;
    }

    #endregion
}