using MauiUiSettings;
using Microsoft.Maui.Layouts;

namespace MauiUiComponents;

public class BasePage<TLayout> : ContentPage
    where TLayout : View, new()
{
    protected internal UiServiceStore _uiServices;
    protected internal ComponentStore _componentStore;

    private readonly Grid _rootLayout;
    public readonly TLayout HostLayout;

    public BasePage(UiServiceStore uiServices, ComponentStore componentStore)
    {
        _uiServices = uiServices;
        _componentStore = componentStore;

        HostLayout = new TLayout();
        _rootLayout = new Grid();

        _rootLayout.AddChild(HostLayout);

        Content = _rootLayout;

        this.ColorBind(_uiServices, x => x.BackgroundColor, ColorVariant.Background);
    }

    public void AddChildren(params View[] views)
    {
        switch (HostLayout)
        {
            case Layout layout:
                foreach (var view in views)
                    layout.Add(view);
                break;

            case ScrollView contentView:
                contentView.Content = views.FirstOrDefault();
                break;

            default:
                throw new InvalidOperationException("Unsupported layout type.");
        }
    }

    public void AddOverlay(
        View view,
        OverlayPlacement placement,
        View? anchor = null)
    {
        if (placement != OverlayPlacement.Center && anchor == null)
            throw new ArgumentNullException(nameof(anchor), "Anchor view must be provided for non-center placements.");

        var overlayLayout = new AbsoluteLayout();
        overlayLayout.ViewOnTapped(
            (_) => _componentStore.Snackbar.Warning("Кнопка не нажата :("),
            (_) => OnOverlayViewClicked(overlayLayout, view));
        view.ViewOnTapped(
            (_) => _componentStore.Snackbar.Success("Кнопка была нажата!"),
            (_) => OnOverlayViewClicked(overlayLayout, view));
        view.ViewAddShadow();

        _rootLayout.AddChild(overlayLayout);
        overlayLayout.Add(view);

        if (placement == OverlayPlacement.Center)
        {
            AbsoluteLayout.SetLayoutFlags(view, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(view, new Rect(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            return;
        }
        else if (placement == OverlayPlacement.BelowAnchor)
        {
            var X = anchor!.X;
            var Y = anchor.Y + anchor.Height;
            AbsoluteLayout.SetLayoutBounds(view, new Rect(X, Y, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        }
    }

    private void OnOverlayViewClicked(AbsoluteLayout layout, View view)
    {
        layout.Remove(view);

        if (_rootLayout.Children.Contains(layout))
            _rootLayout.Children.Remove(layout);
    }
}
