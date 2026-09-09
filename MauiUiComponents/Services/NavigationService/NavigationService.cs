namespace MauiUiComponents;

/// <summary>
/// Управляет глобальной навигацией между представлениями приложения
/// и синхронизирует состояние навигации с <see cref="CustomTitleBar"/>.
/// </summary>
/// <remarks>
/// Сервис не создаёт страницы и представления самостоятельно.
/// Он только переключает содержимое переданного <see cref="ContentView"/>
/// и хранит историю ранее отображённых представлений.
/// </remarks>
public sealed class NavigationService :
    IDisposable
{
    #region Fields

    private readonly Stack<View> _history =
        new();

    private ContentView? _contentHost;
    private CustomTitleBar? _titleBar;
    private View? _shell;

    private bool _initialized;
    private bool _disposed;

    #endregion

    #region Properties

    /// <summary>
    /// Возвращает текущее отображаемое представление.
    /// </summary>
    public View? CurrentView =>
        _contentHost?.Content;

    /// <summary>
    /// Возвращает признак наличия предыдущего
    /// представления в истории навигации.
    /// </summary>
    public bool CanGoBack =>
        _history.Count > 0;

    #endregion

    #region Initialization

    /// <summary>
    /// Инициализирует сервис контейнером содержимого,
    /// пользовательским TitleBar и корневым Shell-представлением.
    /// </summary>
    /// <param name="contentHost">
    /// Контейнер, в котором отображаются навигационные представления.
    /// </param>
    /// <param name="titleBar">
    /// Общий TitleBar приложения.
    /// </param>
    /// <param name="shell">
    /// Корневое представление приложения.
    /// </param>
    public void Initialize(
        ContentView contentHost,
        CustomTitleBar titleBar,
        View shell)
    {
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(contentHost);
        ArgumentNullException.ThrowIfNull(titleBar);
        ArgumentNullException.ThrowIfNull(shell);

        /*
         * Повторная инициализация не должна оставлять
         * подписку на старый экземпляр CustomTitleBar.
         */
        if (_titleBar is not null)
        {
            _titleBar.BackRequested -=
                OnBackRequested;
        }

        _contentHost =
            contentHost;

        _titleBar =
            titleBar;

        _shell =
            shell;

        _titleBar.BackRequested +=
            OnBackRequested;

        _initialized =
            true;

        ShowShell();
    }

    #endregion

    #region Navigation

    /// <summary>
    /// Возвращает приложение к корневому Shell
    /// и полностью очищает историю навигации.
    /// </summary>
    public void ShowShell()
    {
        EnsureInitialized();

        _history.Clear();

        _contentHost!.Content =
            _shell;

        _titleBar!.ShowRoot();
    }

    /// <summary>
    /// Переходит к указанному представлению,
    /// сохраняя текущее представление в истории.
    /// </summary>
    /// <param name="view">
    /// Представление, которое необходимо отобразить.
    /// </param>
    public void Navigate(
        View view)
    {
        EnsureInitialized();

        ArgumentNullException.ThrowIfNull(view);

        var currentView =
            _contentHost!.Content;

        if (ReferenceEquals(
                currentView,
                view))
        {
            return;
        }

        if (currentView is not null)
        {
            _history.Push(
                currentView);
        }

        _contentHost.Content =
            view;

        _titleBar!.ShowBack();
    }

    /// <summary>
    /// Возвращается к предыдущему представлению
    /// в истории навигации.
    /// </summary>
    /// <returns>
    /// <see langword="true"/>, если переход назад был выполнен;
    /// иначе <see langword="false"/>.
    /// </returns>
    public bool GoBack()
    {
        EnsureInitialized();

        if (_history.Count == 0)
            return false;

        var previousView =
            _history.Pop();

        _contentHost!.Content =
            previousView;

        UpdateTitleBar(
            previousView);

        return true;
    }

    #endregion

    #region TitleBar

    /// <summary>
    /// Обновляет режим TitleBar в зависимости от того,
    /// является ли отображаемое представление корневым Shell.
    /// </summary>
    private void UpdateTitleBar(
        View view)
    {
        if (ReferenceEquals(
                view,
                _shell))
        {
            _titleBar!.ShowRoot();

            return;
        }

        _titleBar!.ShowBack();
    }

    /// <summary>
    /// Обрабатывает запрос возврата,
    /// поступивший от <see cref="CustomTitleBar"/>.
    /// </summary>
    private void OnBackRequested(
        object? sender,
        EventArgs e)
    {
        GoBack();
    }

    #endregion

    #region Validation

    /// <summary>
    /// Проверяет, что сервис был инициализирован
    /// перед выполнением навигационной операции.
    /// </summary>
    private void EnsureInitialized()
    {
        ThrowIfDisposed();

        if (_initialized)
            return;

        throw new InvalidOperationException(
            $"{nameof(NavigationService)} не был инициализирован.");
    }

    /// <summary>
    /// Проверяет, что сервис не был освобождён.
    /// </summary>
    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(
            _disposed,
            this);
    }

    #endregion

    #region Dispose

    /// <summary>
    /// Освобождает подписки сервиса
    /// на события пользовательского TitleBar.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        if (_titleBar is not null)
        {
            _titleBar.BackRequested -=
                OnBackRequested;
        }

        _history.Clear();

        _contentHost =
            null;

        _titleBar =
            null;

        _shell =
            null;

        _initialized =
            false;

        _disposed =
            true;

        GC.SuppressFinalize(
            this);
    }

    #endregion
}