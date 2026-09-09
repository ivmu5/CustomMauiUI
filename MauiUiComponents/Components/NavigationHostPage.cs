namespace MauiUiComponents;

/// <summary>
/// Корневая страница навигационной системы приложения.
/// Содержит постоянный <see cref="CustomTitleBar"/>
/// и контейнер для отображения текущего представления.
/// </summary>
/// <remarks>
/// Сама страница не управляет историей переходов.
/// За изменение содержимого <see cref="ContentHost"/>
/// отвечает <see cref="NavigationService"/>.
/// </remarks>
public sealed class NavigationHostPage :
    ContentPage,
    IDisposable
{
    #region Fields

    private readonly ComponentStore _componentStore;
    private readonly Grid _rootGrid;

    private bool _disposed;

    #endregion

    #region Properties

    /// <summary>
    /// Получает постоянную пользовательскую
    /// панель заголовка приложения.
    /// </summary>
    public CustomTitleBar TitleBar { get; }

    /// <summary>
    /// Получает контейнер, в котором отображается
    /// текущее навигационное представление.
    /// </summary>
    public ContentView ContentHost { get; }

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт корневую страницу навигационной системы.
    /// </summary>
    /// <param name="componentStore">
    /// Центральное хранилище UI-компонентов и сервисов.
    /// </param>
    public NavigationHostPage(
        ComponentStore componentStore)
    {
        ArgumentNullException.ThrowIfNull(
            componentStore);

        _componentStore =
            componentStore;

        _rootGrid =
            new Grid();

        TitleBar =
            new CustomTitleBar(
                componentStore);

        ContentHost =
            new ContentView();

        BuildLayout();

        this.ColorBackgroundBind(
            componentStore.UiServices);

        Content =
            _rootGrid;
    }

    #endregion

    #region Layout

    /// <summary>
    /// Формирует структуру корневой страницы:
    /// TitleBar располагается сверху,
    /// а навигационное содержимое занимает оставшееся пространство.
    /// </summary>
    private void BuildLayout()
    {
        _rootGrid
            .AddAutoRow()
            .AddStarRow();

        _rootGrid
            .AddChild(
                TitleBar,
                0,
                0)
            .AddChild(
                ContentHost,
                1,
                0);
    }

    #endregion

    #region Dispose

    /// <summary>
    /// Освобождает ресурсы,
    /// принадлежащие корневой странице.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        /*
         * NavigationHostPage владеет экземпляром CustomTitleBar,
         * поэтому отвечает и за завершение его жизненного цикла.
         */
        TitleBar.Dispose();

        ContentHost.Content =
            null;

        _disposed =
            true;

        GC.SuppressFinalize(
            this);
    }

    #endregion
}