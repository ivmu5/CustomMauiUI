using MauiUiSettings;
using MauiUiSettings.Resources.Localization.MaterialSymbols;
using System.ComponentModel;

namespace MauiUiComponents;

public class CustomTitleBar : ContentView
{
    private readonly ComponentStore _componentStore;

    private readonly Grid _rootGrid;

    public ContentView LogoView { get; }
    public Grid CustomContent { get; }

    public BaseButton MinimizeButton { get; }
    public BaseButton MaximizeButton { get; }
    public BaseButton CloseButton { get; }



    public CustomTitleBar(ComponentStore componentStore)
    {
        _componentStore = componentStore;
        _rootGrid = new Grid();

        _rootGrid
            .AddAutoColumn()
            .AddStarColumn()
            .AddAutoColumn()
            .AddAutoColumn()
            .AddAutoColumn();

        _rootGrid.ColorBackgroundBind(
            componentStore.UiServices,
            ColorVariant.Blur);

        LogoView = new ContentView();
        CustomContent = new Grid();

        MinimizeButton = componentStore.Base
            .Button(ColorVariant.Blur, FontVariant.Icon)
            .TextIconBind(_componentStore, nameof(MaterialSymbols.WindowMinimize))
            .Unbind(x => x.FontSize, 16);

        MaximizeButton = componentStore.Base
            .Button(ColorVariant.Blur, FontVariant.Icon)
            .TextIconBind(_componentStore, nameof(MaterialSymbols.WindowOpenInFull))
            .Unbind(x => x.FontSize, 16);

        CloseButton = componentStore.Base
            .Button(ColorVariant.Blur, FontVariant.Icon)
            .TextIconBind(_componentStore, nameof(MaterialSymbols.WindowClose))
            .Unbind(x => x.FontSize, 16);

        var windowService = _componentStore.UiServices.WindowService;

        MinimizeButton.Clicked += (_, _) =>
            windowService.Minimize();

        MaximizeButton.Clicked += (_, _) =>
            windowService.ToggleMaximize();

        CloseButton.Clicked += (_, _) =>
            windowService.Close();

        _rootGrid
            .AddChild(LogoView, 0, 0)
            .AddChild(CustomContent, 0, 1)
            .AddChild(MinimizeButton, 0, 2)
            .AddChild(MaximizeButton, 0, 3)
            .AddChild(CloseButton, 0, 4);

        Content = _rootGrid;

        windowService.PropertyChanged += OnWindowPropertyChanged;
        _componentStore.UiServices.WindowService.SetCustomTitleBar(this);
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
        MaximizeButton
            .TextIconBind(
            _componentStore,
                _componentStore.UiServices.WindowService.IsMaximized
                    ? nameof(MaterialSymbols.WindowCloseFull)
                    : nameof(MaterialSymbols.WindowOpenInFull));
    }
}