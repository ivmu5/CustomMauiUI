namespace MauiUiComponents;

public interface IOverlayService
{
    void AddOverlay(
        View view,
        OverlayPlacement placement,
        View? anchor = null);

    void RemoveOverlay(View view);
}