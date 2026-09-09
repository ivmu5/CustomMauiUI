using MauiUiSettings;
using SQLiteStorage;

namespace MauiUiComponents;

/// <summary>
/// Корневое навигационное представление основных разделов приложения.
/// Отображает содержимое выбранного раздела
/// и панель переключения между ними.
/// </summary>
/// <typeparam name="TView">
/// Тип визуального элемента, используемого
/// для кнопок основной навигации.
/// </typeparam>
public sealed class CustomShell<TView> :
    ContentView,
    IDisposable
    where TView :
        View,
        ITextComponent,
        new()
{
    #region Fields

    private readonly ComponentStore _componentStore;

    private readonly Dictionary<string, PageShellFactory>
        _pages = new();

    private readonly Grid _rootGrid;

    private readonly ContentView _contentHost;

    private readonly BaseBorder<ToggleGroup<string, FlexLayout>>
        _bottomBarBorder;

    private bool _isLandscape;
    private bool _disposed;

    #endregion

    #region Properties

    /// <summary>
    /// Получает контейнер текущего содержимого Shell.
    /// </summary>
    public ContentView ContentHost =>
        _contentHost;

    /// <summary>
    /// Получает навигационную группу
    /// основных разделов приложения.
    /// </summary>
    public ToggleGroup<string, FlexLayout> NavigationBar =>
        _bottomBarBorder.View;

    /// <summary>
    /// Получает ключ текущего выбранного раздела.
    /// </summary>
    public string? SelectedPage =>
        NavigationBar.SelectedItem;

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт корневое представление
    /// основных разделов приложения.
    /// </summary>
    /// <param name="componentStore">
    /// Центральное хранилище UI-компонентов и сервисов.
    /// </param>
    public CustomShell(
        ComponentStore componentStore)
    {
        ArgumentNullException.ThrowIfNull(
            componentStore);

        _componentStore =
            componentStore;

        _rootGrid =
            new Grid();

        _contentHost =
            new ContentView();

        var navigationBar =
            _componentStore.Custom
                .ToggleGroup
                .ToggleGroup<string, FlexLayout>(
                    Array.Empty<string>(),
                    CreateNavigationToggle);

        _bottomBarBorder =
            navigationBar.WithBorder(
                _componentStore,
                backgroundColor: ColorVariant.Blur);

        BuildLayout();
        SubscribeEvents();

        Content =
            _rootGrid;
    }

    #endregion

    #region Page Registration

    /// <summary>
    /// Регистрирует основной раздел приложения
    /// с иконкой Material Symbols.
    /// </summary>
    /// <param name="viewFactory">
    /// Фабрика представления раздела.
    /// </param>
    /// <param name="iconKey">
    /// Ключ иконки Material Symbols.
    /// </param>
    /// <param name="pageKey">
    /// Уникальный ключ раздела.
    /// </param>
    public void AddIconPage(
        Func<View> viewFactory,
        string iconKey,
        string pageKey)
    {
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(
            viewFactory);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            iconKey);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            pageKey);

        if (_pages.ContainsKey(
                pageKey))
        {
            throw new InvalidOperationException(
                $"Раздел Shell с ключом '{pageKey}' уже зарегистрирован.");
        }

        var factory =
            new PageShellFactory(
                viewFactory,
                () =>
                    _componentStore.Custom
                        .ToggleGroup
                        .BaseIconToggleView<TView>(
                            iconKey));

        _pages.Add(
            pageKey,
            factory);

        RefreshNavigationItems();

        /*
         * Первый зарегистрированный раздел
         * автоматически становится активным.
         */
        if (NavigationBar.SelectedItem is null)
        {
            NavigationBar.SelectedItem =
                pageKey;
        }
    }

    #endregion

    #region Page Navigation

    /// <summary>
    /// Отображает зарегистрированный раздел
    /// с указанным ключом.
    /// </summary>
    /// <param name="pageKey">
    /// Ключ раздела, который необходимо показать.
    /// </param>
    public void Navigate(
        string pageKey)
    {
        ThrowIfDisposed();

        ArgumentException.ThrowIfNullOrWhiteSpace(
            pageKey);

        if (!_pages.TryGetValue(
                pageKey,
                out var factory))
        {
            throw new KeyNotFoundException(
                $"Раздел Shell с ключом '{pageKey}' не зарегистрирован.");
        }

        var view =
            factory.CreateView();

        ArgumentNullException.ThrowIfNull(
            view);

        if (ReferenceEquals(
                _contentHost.Content,
                view))
        {
            return;
        }

        _contentHost.Content =
            view;
    }

    /// <summary>
    /// Обрабатывает изменение выбранного
    /// пункта основной навигации.
    /// </summary>
    private void OnNavigationSelectionChanged(
        object? sender,
        ValueChangedEventArgs<string> e)
    {
        if (sender is not
            ToggleGroup<string, FlexLayout> navigationBar)
        {
            return;
        }

        var pageKey =
            navigationBar.SelectedItem;

        if (string.IsNullOrWhiteSpace(
                pageKey))
        {
            return;
        }

        Navigate(
            pageKey);
    }

    #endregion

    #region Navigation Bar

    /// <summary>
    /// Создаёт toggle-элемент для указанного
    /// зарегистрированного раздела.
    /// </summary>
    private IToggleItem CreateNavigationToggle(
        string pageKey)
    {
        if (!_pages.TryGetValue(
                pageKey,
                out var factory))
        {
            throw new KeyNotFoundException(
                $"Раздел Shell с ключом '{pageKey}' не зарегистрирован.");
        }

        return factory.CreateToggle();
    }

    /// <summary>
    /// Обновляет набор элементов панели навигации
    /// после регистрации нового раздела.
    /// </summary>
    private void RefreshNavigationItems()
    {
        NavigationBar.ItemsSource =
            _pages.Keys.ToList();

        ConfigureNavigationLayout();
    }

    #endregion

    #region Layout

    /// <summary>
    /// Выполняет первоначальную настройку
    /// визуальной структуры Shell.
    /// </summary>
    private void BuildLayout()
    {
        _rootGrid.AddChild(
            _contentHost);

        _rootGrid.AddChild(
            _bottomBarBorder);

        /*
         * До первого SizeChanged размеры Shell ещё неизвестны,
         * поэтому портретная конфигурация используется
         * как безопасное состояние по умолчанию.
         */
        ConfigurePortrait();
    }

    /// <summary>
    /// Сбрасывает параметры расположения элементов
    /// перед применением новой ориентации Shell.
    /// </summary>
    private void ResetLayout()
    {
        _rootGrid.RowDefinitions.Clear();
        _rootGrid.ColumnDefinitions.Clear();

        _contentHost.GridPosition(
            0,
            0);

        _bottomBarBorder.GridPosition(
            0,
            0);

        Grid.SetRowSpan(
            _contentHost,
            1);

        Grid.SetColumnSpan(
            _contentHost,
            1);

        Grid.SetRowSpan(
            _bottomBarBorder,
            1);

        Grid.SetColumnSpan(
            _bottomBarBorder,
            1);

        /*
         * Отступы и параметры выравнивания
         * задаются заново для каждой ориентации.
         */
        _contentHost.Padding =
            Thickness.Zero;

        _contentHost.Margin =
            Thickness.Zero;

        _bottomBarBorder.Padding =
            Thickness.Zero;

        _bottomBarBorder.Margin =
            Thickness.Zero;

        _contentHost.HorizontalOptions =
            LayoutOptions.Fill;

        _contentHost.VerticalOptions =
            LayoutOptions.Fill;

        _bottomBarBorder.HorizontalOptions =
            LayoutOptions.Center;

        _bottomBarBorder.VerticalOptions =
            LayoutOptions.Center;

        NavigationBar.HorizontalOptions =
            LayoutOptions.Center;

        NavigationBar.VerticalOptions =
            LayoutOptions.Center;

        NavigationBar.ToggleLayout.HorizontalOptions =
            LayoutOptions.Center;

        NavigationBar.ToggleLayout.VerticalOptions =
            LayoutOptions.Center;
    }

    /// <summary>
    /// Выбирает подходящую компоновку
    /// на основании текущих размеров Shell.
    /// </summary>
    private void UpdateOrientation()
    {
        if (Width <= 0 ||
            Height <= 0)
        {
            return;
        }

        var isLandscape =
            Width > Height;

        if (_isLandscape == isLandscape)
            return;

        if (isLandscape)
        {
            ConfigureLandscape();

            return;
        }

        ConfigurePortrait();
    }

    /// <summary>
    /// Настраивает Shell для портретной ориентации:
    /// содержимое занимает основную область,
    /// а компактная панель навигации располагается
    /// снизу по центру.
    /// </summary>
    private void ConfigurePortrait()
    {
        ResetLayout();

        _isLandscape =
            false;

        _rootGrid
            .AddStarRow()
            .AddAutoRow();

        _contentHost.GridPosition(
            0,
            0);

        _bottomBarBorder.GridPosition(
            1,
            0);

        _contentHost.Padding =
            new Thickness(
                10);

        _bottomBarBorder.Margin =
            new Thickness(
                10);

        /*
         * BottomBar не растягивается на всю ширину окна.
         * Его размер определяется собственным содержимым.
         */
        _bottomBarBorder.ViewCenter();
        NavigationBar.ViewCenter();
        NavigationBar.ToggleLayout.ViewCenter();

        NavigationBar.ToggleLayout
            .FlexRow();

        ConfigureNavigationLayout();

        _componentStore.UiServices
            .StatusBarService
            .IsVisible = true;

        SnackbarService.SetBaseAnchor(
            _bottomBarBorder);
    }

    /// <summary>
    /// Настраивает Shell для альбомной ориентации:
    /// компактная панель навигации располагается слева,
    /// а содержимое начинается непосредственно рядом с ней.
    /// </summary>
    private void ConfigureLandscape()
    {
        ResetLayout();

        _isLandscape =
            true;

        _rootGrid
            .AddAutoColumn()
            .AddStarColumn();

        _bottomBarBorder.GridPosition(
            0,
            0);

        _contentHost.GridPosition(
            0,
            1);

        /*
         * Правый отступ BottomBar и левый отступ ContentHost
         * намеренно небольшие, чтобы между панелью
         * и содержимым не появлялось большое пустое пространство.
         */
        _bottomBarBorder.Margin =
            new Thickness(
                10,
                10,
                5,
                10);

        _contentHost.Padding =
            new Thickness(
                5,
                10,
                10,
                10);

        /*
         * Высота боковой панели определяется её элементами.
         * Она не должна растягиваться на всю высоту Shell.
         */
        _bottomBarBorder.ViewCenter();
        NavigationBar.ViewCenter();
        NavigationBar.ToggleLayout
            .ViewHorizontalCenter()
            .ViewVerticalStart();

        NavigationBar.ToggleLayout
            .FlexColumn();

        ConfigureNavigationLayout();

        _componentStore.UiServices
            .StatusBarService
            .IsVisible = false;

        SnackbarService.SetBaseAnchor();
    }

    /// <summary>
    /// Настраивает элементы панели навигации так,
    /// чтобы их размер определялся собственным содержимым,
    /// а не свободным пространством FlexLayout.
    /// </summary>
    private void ConfigureNavigationLayout()
    {
        NavigationBar.ToggleLayout.FlexEqualGrow(0);
    }

    #endregion
     
    #region Events

    /// <summary>
    /// Регистрирует обработчики событий Shell.
    /// </summary>
    private void SubscribeEvents()
    {
        NavigationBar.SelectionChanged +=
            OnNavigationSelectionChanged;

        SizeChanged +=
            OnShellSizeChanged;
    }

    /// <summary>
    /// Обрабатывает изменение размеров Shell
    /// и при необходимости перестраивает ориентацию.
    /// </summary>
    private void OnShellSizeChanged(
        object? sender,
        EventArgs e)
    {
        UpdateOrientation();
    }

    #endregion

    #region Validation

    /// <summary>
    /// Проверяет, что Shell ещё не был освобождён.
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
    /// Освобождает обработчики событий Shell
    /// и очищает отображаемое содержимое.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        NavigationBar.SelectionChanged -=
            OnNavigationSelectionChanged;

        SizeChanged -=
            OnShellSizeChanged;

        _contentHost.Content =
            null;

        SnackbarService.SetBaseAnchor();

        _pages.Clear();

        _disposed =
            true;

        GC.SuppressFinalize(
            this);
    }

    #endregion
}