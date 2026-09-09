using ObjectExtensions;

namespace MauiUiComponents;

/// <summary>
/// Предоставляет фабричные методы для создания UI-компонентов,
/// работающих со значениями указанного перечисления.
/// </summary>
/// <typeparam name="TEnum">
/// Тип перечисления, значения которого отображаются в UI.
/// </typeparam>
public static class EnumView<TEnum>
    where TEnum : struct, Enum
{
    #region Toggle Group

    /// <summary>
    /// Создаёт toggle-группу, содержащую все значения
    /// перечисления <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TLayout">
    /// Тип контейнера для размещения toggle-элементов.
    /// </typeparam>
    /// <param name="componentStore">
    /// Центральное хранилище UI-компонентов и сервисов.
    /// </param>
    /// <param name="itemTemplate">
    /// Пользовательская фабрика toggle-элементов.
    /// Если не указана, используется стандартный <see cref="BaseButton"/>.
    /// </param>
    /// <returns>
    /// Toggle-группа, содержащая все значения перечисления.
    /// </returns>
    public static ToggleGroup<TEnum, TLayout> ToggleGroup<TLayout>(
        ComponentStore componentStore,
        Func<TEnum, IToggleItem>? itemTemplate = null)
        where TLayout : Layout, new()
    {
        ArgumentNullException.ThrowIfNull(componentStore);

        itemTemplate ??=
            item => CreateDefaultToggle<BaseButton>(
                item,
                componentStore);

        return componentStore.Custom
            .ToggleGroup
            .ToggleGroup<TEnum, TLayout>(
                Enum.GetValues<TEnum>(),
                itemTemplate);
    }

    #endregion

    #region Dropdown

    /// <summary>
    /// Создаёт dropdown, содержащий все значения
    /// перечисления <typeparamref name="TEnum"/>.
    /// </summary>
    /// <param name="overlayService">
    /// Сервис отображения выпадающего списка.
    /// </param>
    /// <param name="componentStore">
    /// Центральное хранилище UI-компонентов и сервисов.
    /// </param>
    /// <param name="itemTemplate">
    /// Пользовательская фабрика toggle-элементов.
    /// Если не указана, используется стандартный <see cref="BaseButton"/>.
    /// </param>
    /// <returns>
    /// Dropdown, содержащий все значения перечисления.
    /// </returns>
    public static CustomDropdown<TEnum> Dropdown(
        IOverlayService overlayService,
        ComponentStore componentStore,
        Func<TEnum, IToggleItem>? itemTemplate = null)
    {
        ArgumentNullException.ThrowIfNull(overlayService);
        ArgumentNullException.ThrowIfNull(componentStore);

        itemTemplate ??=
            item => CreateDefaultToggle<BaseButton>(
                item,
                componentStore);

        return componentStore.Custom.Dropdown(
            overlayService,
            itemTemplate,
            Enum.GetValues<TEnum>());
    }

    #endregion

    #region Default Toggle

    /// <summary>
    /// Создаёт стандартный toggle-элемент
    /// для указанного значения перечисления.
    /// </summary>
    /// <typeparam name="TView">
    /// Тип визуального представления toggle-элемента.
    /// </typeparam>
    /// <param name="item">
    /// Значение перечисления, которое необходимо отобразить.
    /// </param>
    /// <param name="componentStore">
    /// Центральное хранилище UI-компонентов и сервисов.
    /// </param>
    /// <param name="viewTemplate">
    /// Пользовательская фабрика визуального представления.
    /// Если не указана, создаётся новый экземпляр <typeparamref name="TView"/>.
    /// </param>
    /// <returns>
    /// Toggle-элемент с настроенным текстовым представлением.
    /// </returns>
    public static ToggleItem<TView> CreateDefaultToggle<TView>(
        TEnum item,
        ComponentStore componentStore,
        Func<TView>? viewTemplate = null)
        where TView : View, ITextComponent, new()
    {
        ArgumentNullException.ThrowIfNull(componentStore);

        var view =
            viewTemplate?.Invoke()
            ?? new TView();

        BindItemText(
            view,
            item,
            componentStore);

        return new ToggleItem<TView>(
            view);
    }

    #endregion

    #region Localization

    /// <summary>
    /// Настраивает текстовое представление значения перечисления.
    /// Если значение содержит локализуемый DisplayAttribute,
    /// текст привязывается к соответствующему менеджеру ресурсов.
    /// В противном случае используется строковое представление enum.
    /// </summary>
    private static void BindItemText<TView>(
        TView view,
        TEnum item,
        ComponentStore componentStore)
        where TView : View, ITextComponent
    {
        ArgumentNullException.ThrowIfNull(view);
        ArgumentNullException.ThrowIfNull(componentStore);

        var displayAttribute =
            item.GetDisplayAttribute();

        if (displayAttribute?.ResourceType is not null &&
            !string.IsNullOrWhiteSpace(displayAttribute.Name))
        {
            var localizationManager =
                componentStore.LocalizationStore
                    .GetLocalizationManager(
                        displayAttribute.ResourceType);

            view.TextBind(
                localizationManager,
                displayAttribute.Name);

            return;
        }

        view.Text =
            item.ToString();
    }

    #endregion
}