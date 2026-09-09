namespace MauiUiComponents;

/// <summary>
/// Базовый многострочный текстовый редактор библиотеки MauiUiComponents,
/// построенный на основе стандартного <see cref="Editor"/>.
/// </summary>
/// <remarks>
/// Реализует общие текстовые интерфейсы библиотеки,
/// благодаря чему может использовать единые fluent-расширения
/// для оформления текста и его выравнивания.
/// </remarks>
public class BaseEditor :
    Editor,
    ITextComponent,
    ITextAlignmentComponent
{
}