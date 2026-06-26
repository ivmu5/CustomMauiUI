namespace MauiUiComponents;

public interface ITextComponent
{
    string Text { get; set; }
    Color TextColor { get; set; }
    double FontSize { get; set; }
    string FontFamily { get; set; }
}
