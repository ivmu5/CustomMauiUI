using Microsoft.Maui.Layouts;

namespace MauiUiComponents;

public static class FlexLayoutExtensions
{
    #region Grow / Layout distribution

    /// <summary>
    /// Равномерно распределяет свободное пространство между всеми элементами внутри FlexLayout.
    /// Каждый дочерний элемент получает одинаковый Grow = 1.
    /// Используется для создания равных по размеру элементов (кнопки, табы, переключатели).
    /// </summary>
    public static T FlexEqualGrow<T>(this T layout)
        where T : FlexLayout
    {
        foreach (var child in layout.Children)
            layout.SetGrow(child, 1);

        return layout;
    }

    /// <summary>
    /// Добавляет элементы в FlexLayout и назначает каждому Grow = 1,
    /// обеспечивая равномерное распределение пространства между ними.
    /// </summary>
    public static T AddEqual<T>(this T layout, params View[] views)
        where T : FlexLayout
    {
        foreach (var view in views)
        {
            FlexLayout.SetGrow(view, 1);
            layout.Children.Add(view);
        }

        return layout;
    }

    /// <summary>
    /// Добавляет один элемент в FlexLayout и задаёт ему коэффициент Grow.
    /// Grow определяет, сколько свободного пространства элемент занимает относительно других.
    /// </summary>
    public static T AddGrow<T>(this T layout, View view, int grow = 1)
        where T : FlexLayout
    {
        FlexLayout.SetGrow(view, grow);
        layout.Children.Add(view);
        return layout;
    }

    #endregion

    #region Direction

    /// <summary>
    /// Устанавливает горизонтальное расположение элементов (Row)
    /// и отключает перенос строк.
    /// </summary>
    public static T FlexRow<T>(this T layout)
        where T : FlexLayout
    {
        layout.Direction = FlexDirection.Row;
        layout.Wrap = FlexWrap.NoWrap;
        return layout;
    }

    /// <summary>
    /// Устанавливает вертикальное расположение элементов (Column)
    /// и отключает перенос строк.
    /// </summary>
    public static T FlexColumn<T>(this T layout)
        where T : FlexLayout
    {
        layout.Direction = FlexDirection.Column;
        layout.Wrap = FlexWrap.NoWrap;
        return layout;
    }

    #endregion

    #region Wrap

    /// <summary>
    /// Включает перенос элементов на следующую строку или колонку
    /// при нехватке свободного места.
    /// </summary>
    public static T FlexWrapContent<T>(this T layout)
        where T : FlexLayout
    {
        layout.Wrap = FlexWrap.Wrap;
        return layout;
    }

    #endregion

    #region JustifyContent (main axis alignment)

    /// <summary>
    /// Выравнивание элементов по началу основной оси (Start).
    /// </summary>
    public static T FlexContentStart<T>(this T layout)
        where T : FlexLayout
    {
        layout.JustifyContent = FlexJustify.Start;
        return layout;
    }

    /// <summary>
    /// Центрирует элементы вдоль основной оси.
    /// </summary>
    public static T FlexContentCenter<T>(this T layout)
        where T : FlexLayout
    {
        layout.JustifyContent = FlexJustify.Center;
        return layout;
    }

    /// <summary>
    /// Выравнивает элементы по концу основной оси (End).
    /// </summary>
    public static T FlexContentEnd<T>(this T layout)
        where T : FlexLayout
    {
        layout.JustifyContent = FlexJustify.End;
        return layout;
    }

    /// <summary>
    /// Распределяет элементы вдоль основной оси с равными отступами вокруг каждого элемента.
    /// У первого и последнего элемента также есть отступ от края контейнера.
    /// </summary>
    public static T FlexContentSpaceAround<T>(this T layout)
        where T : FlexLayout
    {
        layout.JustifyContent = FlexJustify.SpaceAround;
        return layout;
    }

    /// <summary>
    /// Распределяет элементы с максимальным расстоянием между ними.
    /// Первый элемент у начала, последний у конца.
    /// </summary>
    public static T FlexContentSpaceBetween<T>(this T layout)
        where T : FlexLayout
    {
        layout.JustifyContent = FlexJustify.SpaceBetween;
        return layout;
    }

    /// <summary>
    /// Равномерно распределяет элементы по всей доступной ширине,
    /// включая отступы по краям.
    /// </summary>
    public static T FlexContentSpaceEvenly<T>(this T layout)
        where T : FlexLayout
    {
        layout.JustifyContent = FlexJustify.SpaceEvenly;
        return layout;
    }

    #endregion

    #region AlignItems (cross axis alignment)

    /// <summary>
    /// Выравнивает элементы по началу поперечной оси.
    /// </summary>
    public static T FlexAlignStart<T>(this T layout)
        where T : FlexLayout
    {
        layout.AlignItems = FlexAlignItems.Start;
        return layout;
    }

    /// <summary>
    /// Центрирует элементы по поперечной оси.
    /// </summary>
    public static T FlexAlignCenter<T>(this T layout)
        where T : FlexLayout
    {
        layout.AlignItems = FlexAlignItems.Center;
        return layout;
    }

    /// <summary>
    /// Выравнивает элементы по концу поперечной оси.
    /// </summary>
    public static T FlexAlignEnd<T>(this T layout)
        where T : FlexLayout
    {
        layout.AlignItems = FlexAlignItems.End;
        return layout;
    }

    /// <summary>
    /// Растягивает элементы по поперечной оси на доступное пространство.
    /// </summary>
    public static T FlexAlignStretch<T>(this T layout)
        where T : FlexLayout
    {
        layout.AlignItems = FlexAlignItems.Stretch;
        return layout;
    }

    #endregion
}