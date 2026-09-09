namespace MauiUiComponents;

/// <summary>
/// Хранит фабрики визуального содержимого раздела Shell
/// и соответствующего элемента навигации.
/// </summary>
public sealed class PageShellFactory
{
    #region Properties

    /// <summary>
    /// Получает фабрику создания визуального содержимого раздела.
    /// </summary>
    public Func<View> ViewFactory { get; }

    /// <summary>
    /// Получает фабрику создания toggle-элемента,
    /// используемого в панели навигации Shell.
    /// </summary>
    public Func<IToggleItem> ToggleFactory { get; }

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт набор фабрик для одного раздела Shell.
    /// </summary>
    /// <param name="viewFactory">
    /// Фабрика визуального содержимого раздела.
    /// </param>
    /// <param name="toggleFactory">
    /// Фабрика элемента навигации раздела.
    /// </param>
    public PageShellFactory(
        Func<View> viewFactory,
        Func<IToggleItem> toggleFactory)
    {
        ArgumentNullException.ThrowIfNull(viewFactory);
        ArgumentNullException.ThrowIfNull(toggleFactory);

        ViewFactory =
            viewFactory;

        ToggleFactory =
            toggleFactory;
    }

    #endregion

    #region Creation

    /// <summary>
    /// Создаёт визуальное содержимое раздела Shell.
    /// </summary>
    public View CreateView()
    {
        var view =
            ViewFactory();

        return view
            ?? throw new InvalidOperationException(
                "Фабрика содержимого Shell вернула null.");
    }

    /// <summary>
    /// Создаёт toggle-элемент панели навигации Shell.
    /// </summary>
    public IToggleItem CreateToggle()
    {
        var toggle =
            ToggleFactory();

        return toggle
            ?? throw new InvalidOperationException(
                "Фабрика элемента навигации Shell вернула null.");
    }

    #endregion
}