using MauiUiSettings;
using System.ComponentModel;

namespace MauiUiComponents;

public class CustomShell<TView> : BasePage<Grid>, IDisposable
    where TView : View, ITextComponent, new()
{
    private readonly Grid _rootGrid;
    private readonly ScrollView _contentScrollView;
    private readonly ContentView _contentHost;
    private readonly BaseBorder<ToggleGroup<string, FlexLayout>> _bottomBarBorder;

    private readonly Dictionary<string, PageShellFactory> _pageShellFactories = new();

    public WindowOrientation CurrentOrientation { get; private set; }



    public CustomShell(ComponentStore componentStore)
        : base(componentStore)
    {
        _rootGrid = new();
        _contentHost = new();
        _contentScrollView = new();
        _bottomBarBorder = componentStore.Custom.ToggleGroup
            .ToggleGroup<string, FlexLayout>(
                new List<string>(),
                route =>
                {
                    var toggleItem = new ToggleItem<TView>();

                    return toggleItem;
                })
            .WithBorder(_componentStore)
            .ColorBackgroundBind(_componentStore.UiServices/*, ColorVariant.Blur*/);

        BuildLayout();
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

        ApplyOrientation(_componentStore.UiServices.WindowService.Orientation);

        _bottomBarBorder.View.ItemTemplate =
            (item) =>
            {
                var pageShellFactory = _pageShellFactories[item];

                var toggleItem = pageShellFactory.PageButtonFactory.Invoke();
                toggleItem.AddAction(
                    new ToggleAction<View>(
                        toggleItem.View,
                        "NavigateShellAction",
                        _ => Navigate(item),
                        ToggleActionTrigger.BusinessAction));

                return toggleItem;
            };

        _componentStore.UiServices.WindowService.PropertyChanged += OnWindowsPropertyChanged;
    }

    private void OnWindowsPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(WindowService.Orientation))
            return;

        SetOrientation(_componentStore.UiServices.WindowService.Orientation);
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

        _componentStore.UiServices.StatusBarService.IsVisible = true;

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

        _componentStore.UiServices.StatusBarService.IsVisible = false;

        SnackbarService.SetBaseAnchor();
    }

    public void AddIconPage(
        Func<ContentPage> pageFactory,
        string iconName,
        string route)
    {
        var toggleItem = _componentStore.Custom.ToggleGroup
            .BaseIconToggleView<TView>(iconName);
        toggleItem.AddAction(
                _componentStore.Custom.ToggleGroup.Styles.ToggleBackgroundColor(toggleItem.View));

        var pageShellFactory = new PageShellFactory(
            pageFactory,
            () =>
            {
                var toggleButton = new ToggleItem<BaseButton>(
                    _componentStore.Base.Button(fontVariant: FontVariant.Icon));
                toggleButton.View.TextIconBind(_componentStore, iconName);

                return toggleButton;
            });

        AddPage(
            pageShellFactory,
            route);
    }

    public void AddPage(
        PageShellFactory pageShellFactory,
        string route)
    {
        if (_pageShellFactories.ContainsKey(route))
            throw new InvalidOperationException(
                $"Page with route '{route}' already exists.");

        _pageShellFactories[route] = pageShellFactory;
        _bottomBarBorder.View.ItemsSource = _pageShellFactories.Keys.ToList();

        if (_contentHost.Content is null)
            _bottomBarBorder.View.SelectedItem = route;
    }



    public void Navigate(string route)
    {
        if (!_pageShellFactories.TryGetValue(route, out var pageShellFactory))
            return;
        var page = pageShellFactory.PageFactory.Invoke();
        _contentHost.Content = page.Content;
    }

    public void Dispose()
    {
        _componentStore.UiServices.WindowService.PropertyChanged -= OnWindowsPropertyChanged;
    }
}