using MauiUiSettings;
using System.ComponentModel;

namespace MauiUiComponents;

public class CustomShell<TView> : BasePage<Grid>, IDisposable
    where TView : View, ITextComponent, new()
{
    public ToggleAction<TView>[] BottomBarToggleColorActions = Array.Empty<ToggleAction<TView>>();

    private readonly Grid _rootGrid;
    private readonly ScrollView _contentScrollView;
    private readonly ContentView _contentHost;
    private readonly BaseBorder<ToggleGroup<FlexLayout>> _bottomBarBorder;

    private readonly Dictionary<string, Func<ContentPage>> _pages = new();

    public WindowOrientation CurrentOrientation { get; private set; }



    public CustomShell(
        UiServiceStore uiServices,
        ComponentStore componentStore)
        : base(uiServices, componentStore)
    {
        this.ColorBind(
            _uiServices,
            x => x.BackgroundColor,
            ColorVariant.Background);

        _rootGrid = new();
        _contentHost = new();
        _contentScrollView = new();
        _bottomBarBorder = componentStore.Custom.ToggleGroup
            .ToggleGroup<FlexLayout>()
            .WithBorder(_componentStore)
            .ColorBackgroundBind(uiServices, ColorVariant.Blur);

        AddBottomBarToggleColorActions();
        BuildLayout();
    }

    private void AddBottomBarToggleColorActions()
    {
        BottomBarToggleColorActions =
        [
            _componentStore.Custom.ToggleGroup.ToggleBackgroundColorAction<TView>(
                ColorVariant.Blur,
                ColorVariant.None),
            _componentStore.Custom.ToggleGroup.ToggleColorAction<TView>(
                x => x.TextColor,
                ColorVariant.Primary,
                ColorVariant.Secondary)
        ];
    }

    private void BuildLayout()
    {
        _bottomBarBorder
            .ViewCenter()
            .ViewAddShadow(
                    radius: 10f,
                    offsetX: 0f,
                    offsetY: 0f);

        _contentScrollView.Content = _contentHost;

        _rootGrid.AddChild(_contentScrollView);
        _rootGrid.AddChild(_bottomBarBorder);

        AddChildren(_rootGrid);

        ApplyOrientation(_uiServices.WindowService.Orientation);
        _uiServices.WindowService.PropertyChanged += OnWindowsPropertyChanged;
    }

    private void OnWindowsPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(WindowService.Orientation))
            return;

        SetOrientation(_uiServices.WindowService.Orientation);
    }

    public void SetOrientation(WindowOrientation orientation)
    {
        if (CurrentOrientation == orientation)
            return;

        ApplyOrientation(orientation);
    }

    private void ApplyOrientation(WindowOrientation orientation)
    {
        CurrentOrientation = orientation;

        switch (CurrentOrientation)
        {
            case WindowOrientation.Horizontal:
                ConfigureLandscape();
                break;

            default:
                ConfigurePortrait();
                break;
        }
    }

    private void ConfigurePortrait()
    {
        _rootGrid.RowDefinitions.Clear();
        _rootGrid.ColumnDefinitions.Clear();

        _rootGrid
            .AddStarRow()
            .AddAutoRow();


        _contentScrollView
            .GridPosition(0, 0)
            .GridRowSpan(2);

        _bottomBarBorder
            .GridPosition(1, 0);

        _contentHost.Padding = 10;
        _bottomBarBorder.Margin = 10;

        _bottomBarBorder.View.ToggleLayout.FlexRow();

        _uiServices.StatusBarService.IsVisible = true;

        SnackbarService.SetBaseAnchor(_bottomBarBorder);
    }

    private void ConfigureLandscape()
    {
        _rootGrid.RowDefinitions.Clear();
        _rootGrid.ColumnDefinitions.Clear();

        _rootGrid
            .AddAutoColumn()
            .AddStarColumn();


        _bottomBarBorder
            .GridPosition(0, 0);

        _contentScrollView
            .GridPosition(0, 1)
            .GridRowSpan(0);

        _contentScrollView.Padding = new Thickness(5, 10, 10, 10);
        _bottomBarBorder.Margin = new Thickness(10, 10, 5, 10); ;

        _bottomBarBorder.View.ToggleLayout.FlexColumn();

        _uiServices.StatusBarService.IsVisible = false;

        SnackbarService.SetBaseAnchor();
    }

    public void AddIconPage(
        Func<ContentPage> pageFactory,
        string iconName,
        string route)
    {
        var toggleGrid = _componentStore.Custom.ToggleGroup
            .BaseIconToggleGrid(
                iconName,
                BottomBarToggleColorActions);

        AddPage(
            pageFactory,
            toggleGrid,
            route);
    }

    public void AddPage(
        Func<ContentPage> pageFactory,
        ToggleGrid toggleGrid,
        string route)
    {
        if (_pages.ContainsKey(route))
            throw new InvalidOperationException(
                $"Page with route '{route}' already exists.");

        _pages[route] = pageFactory;
        _bottomBarBorder.View.AddItem(toggleGrid, () => Navigate(route));

        if (_contentHost.Content is null)
        {
            _bottomBarBorder.View.SelectItem(toggleGrid);
            Navigate(route);
        }
    }



    public void Navigate(string route)
    {
        if (!_pages.TryGetValue(route, out var factory))
            return;

        var page = factory();

        _contentHost.Content = page.Content;
    }

    public void Dispose()
    {
        _uiServices.WindowService.PropertyChanged -= OnWindowsPropertyChanged;
    }
}