using Microsoft.Maui.Layouts;

namespace MauiUiComponents;

public class OverlayService : IOverlayService
{
    private readonly Grid _rootLayout;
    private readonly ComponentStore _componentStore;

    private readonly Dictionary<View, AbsoluteLayout> overlays = new();


    public OverlayService(
        Grid rootLayout,
        ComponentStore componentStore)
    {
        _rootLayout = rootLayout;
        _componentStore = componentStore;
    }


    public void AddOverlay(
        View view,
        OverlayPlacement placement,
        View? anchor = null,
        Action<View>? onOverlayTaped = null)
    {
        if (placement != OverlayPlacement.Center && anchor == null)
            throw new ArgumentNullException(nameof(anchor));

        var overlayLayout = new AbsoluteLayout();
        overlayLayout.ViewOnTapped(
            (_) => RemoveOverlay(view),
            onOverlayTaped);

        view.ViewAddShadow();

        _rootLayout.AddChild(overlayLayout);
        overlayLayout.Add(view);

        overlays[view] = overlayLayout;

        SetPosition(
            view,
            placement,
            anchor);
    }


    private void SetPosition(
        View view,
        OverlayPlacement placement,
        View? anchor)
    {
        if (placement != OverlayPlacement.Center && anchor == null)
            throw new ArgumentNullException(nameof(anchor));

        switch (placement)
        {
            case OverlayPlacement.Center:

                AbsoluteLayout.SetLayoutFlags(
                    view,
                    AbsoluteLayoutFlags.PositionProportional);

                AbsoluteLayout.SetLayoutBounds(
                    view,
                    new Rect(
                        0.5,
                        0.5,
                        AbsoluteLayout.AutoSize,
                        AbsoluteLayout.AutoSize));

                break;


            case OverlayPlacement.BelowAnchor:

                AbsoluteLayout.SetLayoutBounds(
                    view,
                    new Rect(
                        anchor!.X,
                        anchor.Y + anchor.Height,
                        AbsoluteLayout.AutoSize,
                        AbsoluteLayout.AutoSize));

                break;
        }
    }


    public void RemoveOverlay(View view)
    {
        if (!overlays.TryGetValue(view, out var layout))
            return;


        layout.Remove(view);
        _rootLayout.Remove(layout);

        overlays.Remove(view);
    }
}