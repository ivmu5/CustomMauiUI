namespace MauiUiSettings;

public interface IFontService
{
    double FontSize { get; }
    string FontFamily { get; }

    Microsoft.Maui.Font CurrentFont { get; }
    Microsoft.Maui.Font CurrentFontBold { get; }
}
