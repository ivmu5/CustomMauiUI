namespace MauiUiComponents;

public interface IOverlayService
{
    void AddOverlay(
        View view,
        OverlayPlacement placement,
        View? anchor = null,
        Action<View>? onOverlayTaped = null);

    void RemoveOverlay(View view);
}