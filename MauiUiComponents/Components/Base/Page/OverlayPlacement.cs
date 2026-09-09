namespace MauiUiComponents;

/// <summary>
/// Определяет способ расположения overlay-компонента
/// относительно доступной области или указанного элемента.
/// </summary>
public enum OverlayPlacement
{
    /// <summary>
    /// Размещает overlay по центру доступной области.
    /// </summary>
    Center,

    /// <summary>
    /// Размещает overlay непосредственно под элементом,
    /// переданным в качестве anchor.
    /// </summary>
    BelowAnchor,

    /// <summary>
    /// Размещает overlay непосредственно над элементом,
    /// переданным в качестве anchor.
    /// </summary>
    AboveAnchor,

    /// <summary>
    /// Размещает overlay слева от элемента,
    /// переданного в качестве anchor.
    /// </summary>
    LeftOfAnchor,

    /// <summary>
    /// Размещает overlay справа от элемента,
    /// переданного в качестве anchor.
    /// </summary>
    RightOfAnchor
}