namespace MauiUiComponents;

/// <summary>
/// Содержит методы расширения для добавления тени
/// к элементам <see cref="View"/>.
/// </summary>
public static class ViewShadowExtensions
{
    #region Shadow

    /// <summary>
    /// Добавляет тень к элементу.
    /// </summary>
    /// <param name="view">
    /// Элемент, к которому будет добавлена тень.
    /// </param>
    /// <param name="color">
    /// Цвет тени. Если значение не указано,
    /// используется чёрный цвет.
    /// </param>
    /// <param name="radius">
    /// Радиус размытия тени.
    /// </param>
    /// <param name="opacity">
    /// Прозрачность тени в диапазоне от 0 до 1.
    /// </param>
    /// <param name="offsetX">
    /// Горизонтальное смещение тени.
    /// </param>
    /// <param name="offsetY">
    /// Вертикальное смещение тени.
    /// </param>
    /// <returns>
    /// Исходный элемент для продолжения fluent-цепочки.
    /// </returns>
    public static T ViewAddShadow<T>(
        this T view,
        Color? color = null,
        float radius = 15f,
        float opacity = 0.5f,
        float offsetX = 3f,
        float offsetY = 3f)
        where T : View
    {
        ArgumentNullException.ThrowIfNull(view);

        if (radius < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(radius),
                "Радиус тени не может быть отрицательным.");
        }

        if (opacity is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(opacity),
                "Прозрачность тени должна находиться в диапазоне от 0 до 1.");
        }

        view.Shadow =
            new Shadow
            {
                Brush =
                    color ?? Colors.Black,

                Radius =
                    radius,

                Opacity =
                    opacity,

                Offset =
                    new Point(
                        offsetX,
                        offsetY)
            };

        return view;
    }

    #endregion
}