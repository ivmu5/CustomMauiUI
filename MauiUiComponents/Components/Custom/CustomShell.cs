using MauiUiSettings;

namespace MauiUiComponents;

public class CustomShell<TView> : BasePage<Grid>
    where TView : View, ITextComponent, new()
{
    public readonly List<ToggleViewStyle<TView>> BottomBarToggleStyles = new()
    {
        new ToggleViewStyle<TView>(
            x => x.Background,
            ColorVariant.Blur,
            ColorVariant.None),
        new ToggleViewStyle<TView>(
            x => x.TextColor,
            ColorVariant.Primary,
            ColorVariant.Secondary)
    };

    private readonly Grid _rootGrid;
    private readonly ContentView _contentHost;
    private readonly BaseBorder<ToggleGroup<FlexLayout>> _bottomBarBorder;

    private readonly Dictionary<string, Func<ContentPage>> _pages = new();
    private readonly Dictionary<string, ToggleGridView<TView>> _navigationView = new();



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
        _bottomBarBorder = componentStore.Custom.ToggleGroup.ToggleGroup<FlexLayout>().WithBorder(_componentStore);

        _rootGrid.Margin = 10;

        BuildLayout();
    }

    private void BuildLayout()
    {
        _rootGrid
            .AddStarRow()
            .AddAutoRow();

        _bottomBarBorder.ViewCenter();

        _rootGrid.AddChild(_contentHost, 0, rowSpan: 2);
        _rootGrid.AddChild(
            _bottomBarBorder
                .ViewAddShadow(
                    radius: 10f,
                    offsetX: 0f,
                    offsetY: 0f),
            1);

        AddChildren(_rootGrid);

        SnackbarService.SetBaseAnchor(_bottomBarBorder);
    }

    public void AddPage(
        Func<ContentPage> pageFactory,
        ToggleGridView<TView> viewGrid,
        string route)
    {
        if (_pages.ContainsKey(route))
            throw new InvalidOperationException(
                $"Page with route '{route}' already exists.");

        _pages[route] = pageFactory;
        _navigationView[route] = viewGrid;
        _bottomBarBorder.View.AddItem(viewGrid, () => Navigate(route));

        if (_contentHost.Content is null)
        {
            _bottomBarBorder.View.SelectItem(viewGrid);
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
}