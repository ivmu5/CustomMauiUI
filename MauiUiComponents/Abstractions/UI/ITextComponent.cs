namespace MauiUiComponents;

/// <summary>
/// Определяет общий набор текстовых свойств,
/// используемых компонентами библиотеки MauiUiComponents.
/// </summary>
public interface ITextComponent
{
    /// <summary>
    /// Получает или устанавливает отображаемый текст.
    /// </summary>
    string? Text { get; set; }

    /// <summary>
    /// Получает или устанавливает цвет текста.
    /// </summary>
    Color TextColor { get; set; }

    /// <summary>
    /// Получает или устанавливает размер шрифта.
    /// </summary>
    double FontSize { get; set; }

    /// <summary>
    /// Получает или устанавливает семейство шрифта.
    /// </summary>
    string? FontFamily { get; set; }
}