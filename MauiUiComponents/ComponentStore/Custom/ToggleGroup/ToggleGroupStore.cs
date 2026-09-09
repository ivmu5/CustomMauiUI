using MauiUiSettings;

namespace MauiUiComponents;

/// <summary>
/// Предоставляет фабричные методы для создания
/// toggle-групп и стандартных toggle-элементов.
/// </summary>
public sealed class ToggleGroupStore
{
    #region Fields

    private readonly ComponentStore _componentStore;

    #endregion

    #region Properties

    /// <summary>
    /// Получает набор стандартных визуальных действий
    /// для toggle-компонентов.
    /// </summary>
    public ToggleGroupStyleStore Styles { get; }

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт фабрику toggle-компонентов.
    /// </summary>
    /// <param name="componentStore">
    /// Центральное хранилище UI-компонентов и сервисов.
    /// </param>
    public ToggleGroupStore(
        ComponentStore componentStore)
    {
        ArgumentNullException.ThrowIfNull(
            componentStore);

        _componentStore =
            componentStore;

        Styles =
            new ToggleGroupStyleStore(
                componentStore);
    }

    #endregion

    #region Toggle Group

    /// <summary>
    /// Создаёт toggle-группу для указанного набора элементов.
    /// </summary>
    /// <typeparam name="TItem">
    /// Тип значения, представляемого элементами группы.
    /// </typeparam>
    /// <typeparam name="TLayout">
    /// Тип контейнера, используемого для размещения элементов.
    /// </typeparam>
    /// <param name="items">
    /// Набор значений группы.
    /// </param>
    /// <param name="itemTemplate">
    /// Фабрика создания toggle-элемента для каждого значения.
    /// </param>
    /// <param name="selectedItem">
    /// Начальное выбранное значение.
    /// </param>
    /// <returns>
    /// Созданная toggle-группа.
    /// </returns>
    public ToggleGroup<TItem, TLayout> ToggleGroup<TItem, TLayout>(
        IEnumerable<TItem> items,
        Func<TItem, IToggleItem> itemTemplate,
        TItem? selectedItem = default)
        where TItem : notnull
        where TLayout : Layout, new()
    {
        ArgumentNullException.ThrowIfNull(
            items);

        ArgumentNullException.ThrowIfNull(
            itemTemplate);

        return new ToggleGroup<TItem, TLayout>(
            _componentStore,
            itemTemplate)
        {
            SelectedItem =
                selectedItem,

            ItemsSource =
                items.ToList()
        };
    }

    #endregion

    #region Text Toggle

    /// <summary>
    /// Создаёт стандартный текстовый toggle-элемент
    /// с локализуемым содержимым.
    /// </summary>
    /// <typeparam name="TView">
    /// Тип визуального компонента toggle-элемента.
    /// </typeparam>
    /// <param name="localizationManager">
    /// Менеджер ресурсов локализации.
    /// </param>
    /// <param name="localizationKey">
    /// Ключ локализуемого текста.
    /// </param>
    /// <param name="actions">
    /// Дополнительные действия toggle-элемента.
    /// </param>
    /// <returns>
    /// Созданный toggle-элемент.
    /// </returns>
    public ToggleItem<TView> BaseTextToggleView<TView>(
        ILocalizationResourceManager localizationManager,
        string localizationKey,
        params IToggleAction[] actions)
        where TView :
            View,
            ITextComponent,
            new()
    {
        ArgumentNullException.ThrowIfNull(
            localizationManager);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            localizationKey);

        ArgumentNullException.ThrowIfNull(
            actions);

        var toggle =
            new ToggleItem<TView>();

        /*
         * ToggleItem создаёт TView напрямую через new TView(),
         * поэтому стандартный стиль BaseComponentStore
         * автоматически к нему не применяется.
         */
        toggle.View.TextStyleBind(
            _componentStore.UiServices,
            FontVariant.Text);

        toggle.View.TextBind(
            localizationManager,
            localizationKey);

        toggle.AddAction(
            actions);

        return toggle;
    }

    #endregion

    #region Icon Toggle

    /// <summary>
    /// Создаёт стандартный toggle-элемент,
    /// отображающий иконку Material Symbols.
    /// </summary>
    /// <typeparam name="TView">
    /// Тип визуального компонента toggle-элемента.
    /// </typeparam>
    /// <param name="iconKey">
    /// Ключ иконки Material Symbols.
    /// </param>
    /// <param name="actions">
    /// Дополнительные действия toggle-элемента.
    /// </param>
    /// <returns>
    /// Созданный toggle-элемент.
    /// </returns>
    public ToggleItem<TView> BaseIconToggleView<TView>(
        string iconKey,
        params IToggleAction[] actions)
        where TView :
            View,
            ITextComponent,
            new()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            iconKey);

        ArgumentNullException.ThrowIfNull(
            actions);

        var toggle =
            new ToggleItem<TView>();

        /*
         * ToggleItem создаёт TView напрямую,
         * поэтому здесь явно подключаем IconFontService.
         *
         * TextIconBind устанавливает glyph,
         * но сам по себе не меняет FontFamily.
         */
        toggle.View.TextStyleBind(
            _componentStore.UiServices,
            FontVariant.Icon);

        toggle.View.TextIconBind(
            _componentStore,
            iconKey);

        toggle.AddAction(
            actions);

        return toggle;
    }

    #endregion
}