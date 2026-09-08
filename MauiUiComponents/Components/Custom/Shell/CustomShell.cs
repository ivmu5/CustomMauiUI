using MauiUiSettings;
using System.ComponentModel;

namespace MauiUiComponents;

public class CustomShell<TView> : BasePage<Grid>, IDisposable
    where TView : View, ITextComponent, new()
{
    #region Fields

    private readonly Grid _rootGrid;
    private readonly ScrollView _contentScrollView;
    private readonly ContentView _contentHost;
    private readonly BaseBorder<ToggleGroup<string, FlexLayout>> _bottomBarBorder;

    private readonly Dictionary<string, PageShellFactory> _pageShellFactories = new();

    #endregion

    #region Properties

    public WindowOrientation CurrentOrientation { get; private set; }

    public CustomTitleBar TitleBar { get; }

    #endregion



    #region Constructor

    public CustomShell(ComponentStore componentStore)
        : base(componentStore)
    {
        _rootGrid = new();
        _contentHost = new();
        _contentScrollView = new();
        _bottomBarBorder = CreateBottomBar();

        TitleBar = new(_componentStore);
        BuildLayout();
    }

    #endregion

    #region Layout

    private BaseBorder<ToggleGroup<string, FlexLayout>> CreateBottomBar()
    {
        return _componentStore.Custom.ToggleGroup
            .ToggleGroup<string, FlexLayout>(
                new List<string>(),
                _ => new ToggleItem<TView>())
            .WithBorder(_componentStore)
            .ColorBackgroundBind(
                _componentStore.UiServices,
                ColorVariant.Blur)
            .ViewCenter()
            .ViewAddShadow(
                radius: 10f,
                offsetX: 0f,
                offsetY: 0f);
    }

    private void BuildLayout()
    {
        _contentScrollView.Content = _contentHost;

        _rootGrid
            .AddChild(_contentScrollView)
            .AddChild(_bottomBarBorder);

        ConfigureHostLayout();
        ConfigureBottomBar();

        _componentStore.UiServices.WindowService.PropertyChanged +=
            OnWindowPropertyChanged;
    }

    private void ConfigureHostLayout()
    {

        HostLayout
            .AddAutoRow()
            .AddStarRow();

        HostLayout
            .AddChild(_rootGrid)
            .AddChild(TitleBar);

        _rootGrid.GridRowSpan(2);

        TitleBar.SizeChanged += OnTitleBarSizeChanged;
    }

    private void ConfigureBottomBar()
    {
        _bottomBarBorder.View.ItemTemplate =
            item =>
            {
                var pageShellFactory =
                    _pageShellFactories[item];

                var toggleItem =
                    pageShellFactory.PageButtonFactory();

                toggleItem.AddAction(
                    new ToggleAction<View>(
                        toggleItem.View,
                        "NavigateShellAction",
                        _ => Navigate(item),
                        ToggleActionTrigger.BusinessAction));

                return toggleItem;
            };
    }

    #endregion

    #region TitleBar


    private void OnTitleBarSizeChanged(
        object? sender,
        EventArgs e)
    {
        TitleBar.SizeChanged -= OnTitleBarSizeChanged;
        TitleBar.LogoView.HeightRequest = TitleBar.Height;

        ApplyOrientation(
            _componentStore.UiServices.WindowService.Orientation);
    }

    #endregion

    #region Orientation

    private void OnWindowPropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(WindowService.Orientation))
            return;

        SetOrientation(
            _componentStore.UiServices.WindowService.Orientation);
    }

    public void SetOrientation(
        WindowOrientation orientation)
    {
        if (CurrentOrientation == orientation)
            return;

        ApplyOrientation(orientation);
    }

    private void ApplyOrientation(
        WindowOrientation orientation)
    {
        CurrentOrientation = orientation;

        ResetLayout();

        switch (orientation)
        {
            case WindowOrientation.Horizontal:
                ConfigureLandscape();
                break;

            default:
                ConfigurePortrait();
                break;
        }
    }

    private void ResetLayout()
    {
        _rootGrid.RowDefinitions.Clear();
        _rootGrid.ColumnDefinitions.Clear();

        _contentScrollView
            .GridPosition(0, 0)
            .GridRowSpan(1);

        _bottomBarBorder
            .GridPosition(0, 0)
            .GridRowSpan(1);

        _contentScrollView.Padding = 0;
        _contentScrollView.Margin = 0;

        _bottomBarBorder.Padding = 0;
        _bottomBarBorder.Margin = 0;
    }

    private void ConfigurePortrait()
    {
        _rootGrid
            .AddStarRow()
            .AddAutoRow();

        _contentScrollView
            .GridPosition(0, 0)
            .GridRowSpan(2);

        _bottomBarBorder
            .GridPosition(1, 0);

        _contentScrollView.Padding =
            CreateContentPadding(10);

        _bottomBarBorder.Margin =
            new Thickness(10);

        _bottomBarBorder.View
            .ToggleLayout
            .FlexRow();

        _componentStore.UiServices
            .StatusBarService
            .IsVisible = true;

        SnackbarService.SetBaseAnchor(
            _bottomBarBorder);
    }

    private void ConfigureLandscape()
    {
        _rootGrid
            .AddAutoColumn()
            .AddStarColumn();

        _bottomBarBorder
            .GridPosition(0, 0);

        _contentScrollView
            .GridPosition(0, 1);

        _contentScrollView.Padding =
            CreateContentPadding(5);

        _bottomBarBorder.Margin =
            new Thickness(
                10,
                10,
                5,
                10);

        _bottomBarBorder.View
            .ToggleLayout
            .FlexColumn();

        _componentStore.UiServices
            .StatusBarService
            .IsVisible = false;

        SnackbarService.SetBaseAnchor();
    }

    private Thickness CreateContentPadding(
        double left = 10,
        double right = 10,
        double bottom = 10)
    {
        return new Thickness(
            left,
            Math.Max(TitleBar.Height, 10),
            10,
            10);
    }

    #endregion

    #region Pages

    public void AddIconPage(
        Func<ContentPage> pageFactory,
        string iconName,
        string route)
    {
        var toggleItem =
            _componentStore.Custom.ToggleGroup
                .BaseIconToggleView<TView>(iconName);

        toggleItem.AddAction(
            _componentStore.Custom.ToggleGroup.Styles
                .ToggleBackgroundColor(toggleItem.View));

        var pageShellFactory =
            new PageShellFactory(
                pageFactory,
                () => CreatePageButton(iconName));

        AddPage(
            pageShellFactory,
            route);
    }

    private ToggleItem<BaseButton> CreatePageButton(
        string iconName)
    {
        var button =
            _componentStore.Base
                .Button(fontVariant: FontVariant.Icon);

        button.TextIconBind(
            _componentStore,
            iconName);

        return new ToggleItem<BaseButton>(button);
    }

    public void AddPage(
        PageShellFactory pageShellFactory,
        string route)
    {
        ArgumentNullException.ThrowIfNull(pageShellFactory);
        ArgumentException.ThrowIfNullOrWhiteSpace(route);

        if (!_pageShellFactories.TryAdd(
            route,
            pageShellFactory))
        {
            throw new InvalidOperationException(
                $"Page with route '{route}' already exists.");
        }

        _bottomBarBorder.View.ItemsSource =
            _pageShellFactories.Keys.ToList();

        if (_contentHost.Content is null)
        {
            _bottomBarBorder.View.SelectedItem =
                route;
        }
    }

    public void Navigate(string route)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(route);

        if (!_pageShellFactories.TryGetValue(
                route,
                out var pageShellFactory))
        {
            return;
        }

        var page =
            pageShellFactory.PageFactory();

        _contentHost.Content =
            page.Content;
    }

    #endregion

    #region Dispose

    public void Dispose()
    {
        _componentStore.UiServices
            .WindowService
            .PropertyChanged -= OnWindowPropertyChanged;

        TitleBar.SizeChanged -= OnTitleBarSizeChanged;
    }

    #endregion
}