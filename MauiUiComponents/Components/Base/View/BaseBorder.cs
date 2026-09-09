namespace MauiUiComponents;

/// <summary>
/// Типизированная обёртка над <see cref="Border"/>,
/// которая хранит ссылку на вложенное представление исходного типа.
/// </summary>
/// <typeparam name="TView">
/// Тип представления, размещённого внутри рамки.
/// </typeparam>
public sealed class BaseBorder<TView> : Border
    where TView : View
{
    #region Properties

    /// <summary>
    /// Вложенное представление, отображаемое внутри рамки.
    /// </summary>
    public TView View { get; }

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт рамку и помещает внутрь неё указанное представление.
    /// </summary>
    /// <param name="view">
    /// Представление, которое будет использоваться как содержимое рамки.
    /// </param>
    public BaseBorder(
        TView view)
    {
        ArgumentNullException.ThrowIfNull(view);

        View = view;
        Content = view;
    }

    #endregion
}