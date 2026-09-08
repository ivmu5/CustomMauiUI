using MauiUiSettings;
using MauiUiSettings.Resources.Localization.MaterialSymbols;
using System.ComponentModel;

namespace MauiUiComponents;

public class CustomTitleBar : ContentView
{
    private readonly ComponentStore _componentStore;
    private readonly Grid _rootGrid;

    private BaseButton _minimizeButton = null!;
    private BaseButton _maximizeButton = null!;
    private BaseButton _closeButton = null!;

    public ContentView LogoView { get; }
    public Grid CustomContent { get; }

    private bool IsDesktopPlatform =>
        DeviceInfo.Platform == DevicePlatform.WinUI || DeviceInfo.Platform == DevicePlatform.MacCatalyst;


    public CustomTitleBar(ComponentStore componentStore)
    {
        _componentStore = componentStore;

        LogoView = new ContentView()
        {
            MaximumHeightRequest = 40,
            Margin = new Thickness(10, 2.5, 10, 2.5)
        };
        CustomContent = new Grid();

        InitializeButtons();

        _rootGrid = new Grid();

        ConfigureLayout();
        ConfigureEvents();

        this.ColorBackgroundBind(
            _componentStore.UiServices,
            ColorVariant.Blur);

        Content = _rootGrid;
        _componentStore.UiServices.WindowService.SetCustomTitleBar(this);
    }


    private void InitializeButtons()
    {
        _minimizeButton = _componentStore.Base
            .Button(ColorVariant.Blur, FontVariant.Icon)
            .TextIconBind(
                _componentStore,
                nameof(MaterialSymbols.WindowMinimize))
            .Unbind(x => x.FontSize, 16);

        _maximizeButton = _componentStore.Base
            .Button(ColorVariant.Blur, FontVariant.Icon)
            .Unbind(x => x.FontSize, 16);

        _closeButton = _componentStore.Base
            .Button(ColorVariant.Blur, FontVariant.Icon)
            .TextIconBind(
                _componentStore,
                nameof(MaterialSymbols.WindowClose))
            .Unbind(x => x.FontSize, 16);

        UpdateMaximizeIcon();
    }


    private void ConfigureLayout()
    {
        _rootGrid.RowDefinitions.Clear();
        _rootGrid.ColumnDefinitions.Clear();

        _rootGrid
            .AddAutoColumn()
            .AddStarColumn();

        _rootGrid
            .AddChild(LogoView, 0, 0)
            .AddChild(CustomContent, 0, 1);

        if (!IsDesktopPlatform)
            return;

        _rootGrid
            .AddAutoColumn()
            .AddAutoColumn()
            .AddAutoColumn();

        _rootGrid
            .AddChild(_minimizeButton, 0, 2)
            .AddChild(_maximizeButton, 0, 3)
            .AddChild(_closeButton, 0, 4);
    }


    private void ConfigureEvents()
    {
        var windowService = _componentStore.UiServices.WindowService;

        _minimizeButton.Clicked += (_, _) =>
            windowService.Minimize();

        _maximizeButton.Clicked += (_, _) =>
            windowService.ToggleMaximize();

        _closeButton.Clicked += (_, _) =>
            windowService.Close();

        windowService.PropertyChanged += OnWindowPropertyChanged;
    }


    private void OnWindowPropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(WindowService.IsMaximized))
            return;

        UpdateMaximizeIcon();
    }


    private void UpdateMaximizeIcon()
    {
        _maximizeButton.TextIconBind(
            _componentStore,
            _componentStore.UiServices.WindowService.IsMaximized
                ? nameof(MaterialSymbols.WindowCloseFull)
                : nameof(MaterialSymbols.WindowOpenInFull));
    }
}