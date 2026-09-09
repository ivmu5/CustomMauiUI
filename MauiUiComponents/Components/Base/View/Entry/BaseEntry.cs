namespace MauiUiComponents;

/// <summary>
/// Базовое однострочное поле ввода библиотеки MauiUiComponents,
/// построенное на основе стандартного <see cref="Entry"/>.
/// </summary>
/// <remarks>
/// Реализует общие текстовые интерфейсы библиотеки и служит
/// базовым типом для специализированных полей ввода значений.
/// </remarks>
public class BaseEntry :
    Entry,
    ITextComponent,
    ITextAlignmentComponent
{
}