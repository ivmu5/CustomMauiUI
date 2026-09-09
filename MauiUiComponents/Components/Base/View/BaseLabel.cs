namespace MauiUiComponents;

/// <summary>
/// Базовый текстовый компонент библиотеки MauiUiComponents,
/// построенный на основе стандартного <see cref="Label"/>.
/// </summary>
/// <remarks>
/// Реализует общие текстовые интерфейсы библиотеки,
/// благодаря чему может использоваться с едиными
/// fluent-расширениями для текста и его выравнивания.
/// </remarks>
public class BaseLabel :
    Label,
    ITextComponent,
    ITextAlignmentComponent
{
}