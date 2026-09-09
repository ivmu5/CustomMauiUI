namespace MauiUiComponents;

/// <summary>
/// Базовое представление библиотеки MauiUiComponents,
/// содержащее основной пользовательский layout
/// и слой для отображения overlay-компонентов.
/// </summary>
/// <typeparam name="TLayout">
/// Тип основного контейнера представления.
/// </typeparam>
public class BasePage<TLayout> : ContentView
    where TLayout : View, new()
{
    #region Fields

    /// <summary>
    /// Центральное хранилище UI-компонентов и сервисов,
    /// доступное наследникам.
    /// </summary>
    protected internal readonly ComponentStore _componentStore;

    private readonly Grid _rootGrid;

    #endregion

    #region Properties

    /// <summary>
    /// Получает основной контейнер содержимого представления.
    /// </summary>
    public TLayout HostLayout { get; }

    /// <summary>
    /// Получает сервис управления overlay-компонентами
    /// текущего представления.
    /// </summary>
    public OverlayService OverlayService { get; }

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт базовое представление с основным содержимым
    /// и отдельным overlay-слоем.
    /// </summary>
    /// <param name="componentStore">
    /// Центральное хранилище UI-компонентов и сервисов.
    /// </param>
    public BasePage(
        ComponentStore componentStore)
    {
        ArgumentNullException.ThrowIfNull(componentStore);

        _componentStore =
            componentStore;

        _rootGrid =
            new Grid();

        HostLayout =
            new TLayout();

        OverlayService =
            new OverlayService(
                _rootGrid,
                componentStore);

        _rootGrid.Add(
            HostLayout);

        Content =
            _rootGrid;

        this.ColorBackgroundBind(
            componentStore.UiServices);
    }

    #endregion
}