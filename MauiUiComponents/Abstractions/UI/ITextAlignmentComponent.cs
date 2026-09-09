namespace MauiUiComponents;

/// <summary>
/// Определяет общий контракт для компонентов,
/// поддерживающих горизонтальное и вертикальное выравнивание текста.
/// </summary>
public interface ITextAlignmentComponent
{
    /// <summary>
    /// Получает или устанавливает горизонтальное
    /// выравнивание текста внутри компонента.
    /// </summary>
    TextAlignment HorizontalTextAlignment { get; set; }

    /// <summary>
    /// Получает или устанавливает вертикальное
    /// выравнивание текста внутри компонента.
    /// </summary>
    TextAlignment VerticalTextAlignment { get; set; }
}