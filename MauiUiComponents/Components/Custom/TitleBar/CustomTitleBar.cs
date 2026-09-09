using MauiUiSettings;
using MauiUiSettings.Resources.Localization.MaterialSymbols;
using System.ComponentModel;

namespace MauiUiComponents;

/// <summary>
/// Пользовательская панель заголовка окна,
/// содержащая навигационную область, пользовательский контент
/// и системные кнопки управления окном.
/// </summary>
public sealed class CustomTitleBar :
    ContentView,
    IDisposable
{
    #region Fields

    private readonly ComponentStore _componentStore;
    private readonly WindowService _windowService;

    private readonly Grid _rootGrid;
    private readonly ContentView _navigationHost;

    private readonly BaseButton _backButton;
    private readonly BaseButton _minimizeButton;
    private readonly BaseButton _maximizeButton;
    private readonly BaseButton _closeButton;

    private bool _disposed;

    #endregion

    #region Properties

    /// <summary>
    /// Получает контейнер логотипа,
    /// отображаемый в корневом режиме навигации.
    /// </summary>
    public ContentView LogoView { get; }

    /// <summary>
    /// Получает область для пользовательского
    /// содержимого TitleBar.
    /// </summary>
    public Grid CustomContent { get; }

    /// <summary>
    /// Получает текущий режим навигационной
    /// части TitleBar.
    /// </summary>
    public TitleBarNavigationMode NavigationMode
    {
        get;
        private set;
    }

    /// <summary>
    /// Определяет, поддерживает ли текущая платформа
    /// используемую реализацию управления окном.
    /// </summary>
    private static bool SupportsWindowControls =>
        DeviceInfo.Platform == DevicePlatform.WinUI;

    #endregion

    #region Events

    /// <summary>
    /// Возникает при нажатии пользователем
    /// кнопки возврата.
    /// </summary>
    public event EventHandler? BackRequested;

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт пользовательскую панель заголовка окна.
    /// </summary>
    /// <param name="componentStore">
    /// Центральное хранилище UI-компонентов и сервисов.
    /// </param>
    public CustomTitleBar(
        ComponentStore componentStore)
    {
        ArgumentNullException.ThrowIfNull(
            componentStore);

        _componentStore =
            componentStore;

        _windowService =
            componentStore.UiServices.WindowService;

        _rootGrid =
            new Grid();

        _navigationHost =
            new ContentView();

        LogoView =
            new ContentView
            {
                MaximumHeightRequest =
                    40,

                Margin =
                    new Thickness(
                        10,
                        2.5,
                        10,
                        2.5)
            };

        CustomContent =
            new Grid();

        _backButton =
            CreateBackButton();

        _minimizeButton =
            CreateMinimizeButton();

        _maximizeButton =
            CreateMaximizeButton();

        _closeButton =
            CreateCloseButton();

        ConfigureLayout();
        SubscribeEvents();

        this.ColorBackgroundBind(
            _componentStore.UiServices,
            ColorVariant.Blur);

        Content =
            _rootGrid;

        ShowRoot();

        if (SupportsWindowControls)
        {
            _windowService.SetCustomTitleBar(
                this);
        }
    }

    #endregion

    #region Button Creation

    /// <summary>
    /// Создаёт кнопку возврата.
    /// </summary>
    private BaseButton CreateBackButton()
    {
        var button =
            _componentStore.Base
                .Button(
                    ColorVariant.Blur,
                    FontVariant.Icon)
                .TextIconBind(
                    _componentStore,
                    nameof(MaterialSymbols.ArrowLeft))
                .Unbind(
                    target => target.FontSize);

        button.FontSize =
            20;

        return button;
    }

    /// <summary>
    /// Создаёт кнопку минимизации окна.
    /// </summary>
    private BaseButton CreateMinimizeButton()
    {
        var button =
            _componentStore.Base
                .Button(
                    ColorVariant.Blur,
                    FontVariant.Icon)
                .TextIconBind(
                    _componentStore,
                    nameof(MaterialSymbols.WindowMinimize))
                .Unbind(
                    target => target.FontSize);

        button.FontSize =
            16;

        return button;
    }

    /// <summary>
    /// Создаёт кнопку переключения
    /// между обычным и развёрнутым состоянием окна.
    /// </summary>
    private BaseButton CreateMaximizeButton()
    {
        var button =
            _componentStore.Base
                .Button(
                    ColorVariant.Blur,
                    FontVariant.Icon)
                .Unbind(
                    target => target.FontSize);

        button.FontSize =
            16;

        UpdateMaximizeIcon(
            button);

        return button;
    }

    /// <summary>
    /// Создаёт кнопку закрытия окна.
    /// </summary>
    private BaseButton CreateCloseButton()
    {
        var button =
            _componentStore.Base
                .Button(
                    ColorVariant.Blur,
                    FontVariant.Icon)
                .TextIconBind(
                    _componentStore,
                    nameof(MaterialSymbols.WindowClose))
                .Unbind(
                    target => target.FontSize);

        button.FontSize =
            16;

        return button;
    }

    #endregion

    #region Layout

    /// <summary>
    /// Формирует структуру TitleBar
    /// для текущей платформы.
    /// </summary>
    private void ConfigureLayout()
    {
        _rootGrid
            .AddAutoColumn()
            .AddStarColumn();

        _rootGrid
            .AddChild(
                _navigationHost,
                0,
                0)
            .AddChild(
                CustomContent,
                0,
                1);

        if (!SupportsWindowControls)
            return;

        _rootGrid
            .AddAutoColumn()
            .AddAutoColumn()
            .AddAutoColumn();

        _rootGrid
            .AddChild(
                _minimizeButton,
                0,
                2)
            .AddChild(
                _maximizeButton,
                0,
                3)
            .AddChild(
                _closeButton,
                0,
                4);
    }

    #endregion

    #region Navigation

    /// <summary>
    /// Переключает TitleBar в корневой режим,
    /// отображая логотип вместо кнопки возврата.
    /// </summary>
    public void ShowRoot()
    {
        ThrowIfDisposed();

        NavigationMode =
            TitleBarNavigationMode.Root;

        _navigationHost.Content =
            LogoView;
    }

    /// <summary>
    /// Переключает TitleBar в режим возврата,
    /// отображая кнопку Back вместо логотипа.
    /// </summary>
    public void ShowBack()
    {
        ThrowIfDisposed();

        NavigationMode =
            TitleBarNavigationMode.Back;

        _navigationHost.Content =
            _backButton;
    }

    #endregion

    #region Events

    /// <summary>
    /// Регистрирует обработчики событий TitleBar
    /// и состояния окна.
    /// </summary>
    private void SubscribeEvents()
    {
        _backButton.Clicked +=
            OnBackButtonClicked;

        if (!SupportsWindowControls)
            return;

        _minimizeButton.Clicked +=
            OnMinimizeButtonClicked;

        _maximizeButton.Clicked +=
            OnMaximizeButtonClicked;

        _closeButton.Clicked +=
            OnCloseButtonClicked;

        _windowService.PropertyChanged +=
            OnWindowPropertyChanged;
    }

    /// <summary>
    /// Передаёт запрос возврата владельцу TitleBar.
    /// </summary>
    private void OnBackButtonClicked(
        object? sender,
        EventArgs e)
    {
        BackRequested?.Invoke(
            this,
            EventArgs.Empty);
    }

    /// <summary>
    /// Минимизирует текущее окно.
    /// </summary>
    private void OnMinimizeButtonClicked(
        object? sender,
        EventArgs e)
    {
        _windowService.Minimize();
    }

    /// <summary>
    /// Переключает максимизированное состояние окна.
    /// </summary>
    private void OnMaximizeButtonClicked(
        object? sender,
        EventArgs e)
    {
        _windowService.ToggleMaximize();
    }

    /// <summary>
    /// Закрывает текущее окно.
    /// </summary>
    private void OnCloseButtonClicked(
        object? sender,
        EventArgs e)
    {
        _windowService.Close();
    }

    /// <summary>
    /// Обрабатывает изменение состояния окна
    /// и обновляет соответствующие элементы TitleBar.
    /// </summary>
    private void OnWindowPropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        if (e.PropertyName !=
            nameof(WindowService.IsMaximized))
        {
            return;
        }

        UpdateMaximizeIcon(
            _maximizeButton);
    }

    #endregion

    #region Window State

    /// <summary>
    /// Обновляет иконку кнопки максимизации
    /// в соответствии с текущим состоянием окна.
    /// </summary>
    private void UpdateMaximizeIcon(
        BaseButton button)
    {
        ArgumentNullException.ThrowIfNull(
            button);

        var icon =
            _windowService.IsMaximized
                ? nameof(MaterialSymbols.WindowCloseFull)
                : nameof(MaterialSymbols.WindowOpenInFull);

        button.TextIconBind(
            _componentStore,
            icon);
    }

    #endregion

    #region Validation

    /// <summary>
    /// Проверяет, что TitleBar ещё не был освобождён.
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
    /// Освобождает зарегистрированные обработчики событий.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        _backButton.Clicked -=
            OnBackButtonClicked;

        if (SupportsWindowControls)
        {
            _minimizeButton.Clicked -=
                OnMinimizeButtonClicked;

            _maximizeButton.Clicked -=
                OnMaximizeButtonClicked;

            _closeButton.Clicked -=
                OnCloseButtonClicked;

            _windowService.PropertyChanged -=
                OnWindowPropertyChanged;
        }

        _disposed =
            true;

        GC.SuppressFinalize(
            this);
    }

    #endregion
}