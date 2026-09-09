namespace MauiUiComponents;

/// <summary>
/// Предоставляет фабричные методы для создания
/// пользовательских UI-компонентов библиотеки MauiUiComponents.
/// </summary>
public sealed class CustomComponentStore
{
    #region Fields

    private readonly ComponentStore _componentStore;

    #endregion

    #region Properties

    /// <summary>
    /// Получает фабрику toggle-групп,
    /// toggle-элементов и связанных с ними стилей.
    /// </summary>
    public ToggleGroupStore ToggleGroup { get; }

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт фабрику пользовательских UI-компонентов.
    /// </summary>
    /// <param name="componentStore">
    /// Центральное хранилище UI-компонентов и сервисов.
    /// </param>
    public CustomComponentStore(
        ComponentStore componentStore)
    {
        ArgumentNullException.ThrowIfNull(componentStore);

        _componentStore =
            componentStore;

        ToggleGroup =
            new ToggleGroupStore(
                componentStore);
    }

    #endregion

    #region Text Slider

    /// <summary>
    /// Создаёт составной компонент для редактирования
    /// числового значения с помощью текстового поля и слайдера.
    /// </summary>
    /// <typeparam name="TValue">
    /// Тип числового значения компонента.
    /// </typeparam>
    /// <returns>
    /// Новый экземпляр <see cref="CustomTextSlider{TValue}"/>.
    /// </returns>
    public CustomTextSlider<TValue> TextSlider<TValue>()
        where TValue :
            System.Numerics.INumber<TValue>,
            System.Numerics.IMinMaxValue<TValue>
    {
        return new CustomTextSlider<TValue>(
            _componentStore);
    }

    #endregion

    #region Dropdown

    /// <summary>
    /// Создаёт dropdown-компонент для указанного набора элементов.
    /// </summary>
    /// <typeparam name="TItem">
    /// Тип значения, отображаемого в dropdown.
    /// </typeparam>
    /// <param name="overlayService">
    /// Сервис, используемый для отображения выпадающего списка.
    /// </param>
    /// <param name="itemTemplate">
    /// Фабрика создания toggle-элемента
    /// для каждого значения dropdown.
    /// </param>
    /// <param name="items">
    /// Элементы, доступные для выбора.
    /// </param>
    /// <returns>
    /// Новый экземпляр <see cref="CustomDropdown{TItem}"/>.
    /// </returns>
    public CustomDropdown<TItem> Dropdown<TItem>(
        IOverlayService overlayService,
        Func<TItem, IToggleItem> itemTemplate,
        params TItem[] items)
        where TItem : notnull
    {
        ArgumentNullException.ThrowIfNull(overlayService);
        ArgumentNullException.ThrowIfNull(itemTemplate);
        ArgumentNullException.ThrowIfNull(items);

        return new CustomDropdown<TItem>(
            overlayService,
            _componentStore,
            itemTemplate)
        {
            ItemsSource =
                items
        };
    }

    #endregion
}