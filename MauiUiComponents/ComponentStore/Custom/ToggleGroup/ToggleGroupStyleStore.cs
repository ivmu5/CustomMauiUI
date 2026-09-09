using MauiUiSettings;
using System.Linq.Expressions;

namespace MauiUiComponents;

/// <summary>
/// Предоставляет стандартные визуальные действия
/// для элементов <see cref="ToggleGroup{TItem, TLayout}"/>.
/// </summary>
/// <remarks>
/// Создаваемые действия не изменяют состояние выбора самостоятельно.
/// Они только обновляют оформление представления в соответствии
/// с текущим значением <see cref="IToggleItem.IsSelected"/>.
/// </remarks>
public sealed class ToggleGroupStyleStore
{
    #region Fields

    private readonly ComponentStore _componentStore;

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт хранилище стандартных визуальных действий
    /// для toggle-компонентов.
    /// </summary>
    /// <param name="componentStore">
    /// Центральное хранилище UI-компонентов и сервисов.
    /// </param>
    public ToggleGroupStyleStore(
        ComponentStore componentStore)
    {
        ArgumentNullException.ThrowIfNull(componentStore);

        _componentStore =
            componentStore;
    }

    #endregion

    #region Color

    /// <summary>
    /// Создаёт toggle-действие, изменяющее указанное цветовое
    /// свойство представления в зависимости от состояния выбора.
    /// </summary>
    /// <typeparam name="TView">
    /// Тип представления, для которого создаётся действие.
    /// </typeparam>
    /// <param name="view">
    /// Представление, цветовое свойство которого необходимо изменять.
    /// </param>
    /// <param name="propertyExpression">
    /// Выражение, указывающее цветовое свойство представления.
    /// </param>
    /// <param name="selectedColor">
    /// Цветовой вариант для выбранного состояния.
    /// </param>
    /// <param name="unselectedColor">
    /// Цветовой вариант для невыбранного состояния.
    /// </param>
    /// <returns>
    /// Действие, которое можно зарегистрировать
    /// в <see cref="IToggleItem"/>.
    /// </returns>
    public ToggleAction<TView> ToggleColor<TView>(
        TView view,
        Expression<Func<TView, object?>> propertyExpression,
        ColorVariant selectedColor = ColorVariant.Primary,
        ColorVariant unselectedColor = ColorVariant.Secondary)
        where TView : View
    {
        ArgumentNullException.ThrowIfNull(view);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        var propertyName =
            propertyExpression.GetPropertyName();

        return new ToggleAction<TView>(
            view,
            $"ToggleColor:{propertyName}",
            selectedView =>
                selectedView.ColorBind(
                    _componentStore.UiServices,
                    propertyExpression,
                    selectedColor),
            unselectedView =>
                unselectedView.ColorBind(
                    _componentStore.UiServices,
                    propertyExpression,
                    unselectedColor),
            ToggleActionTrigger.Initialization,
            ToggleActionTrigger.SelectionStateChanged);
    }

    /// <summary>
    /// Создаёт toggle-действие, изменяющее цвет фона
    /// представления в зависимости от состояния выбора.
    /// </summary>
    /// <typeparam name="TView">
    /// Тип представления, для которого создаётся действие.
    /// </typeparam>
    /// <param name="view">
    /// Представление, цвет фона которого необходимо изменять.
    /// </param>
    /// <param name="selectedColor">
    /// Цвет фона для выбранного состояния.
    /// </param>
    /// <param name="unselectedColor">
    /// Цвет фона для невыбранного состояния.
    /// </param>
    /// <returns>
    /// Действие изменения цвета фона.
    /// </returns>
    public ToggleAction<TView> ToggleBackgroundColor<TView>(
        TView view,
        ColorVariant selectedColor = ColorVariant.Primary,
        ColorVariant unselectedColor = ColorVariant.Secondary)
        where TView : View
    {
        return ToggleColor(
            view,
            target => target.BackgroundColor,
            selectedColor,
            unselectedColor);
    }

    #endregion
}