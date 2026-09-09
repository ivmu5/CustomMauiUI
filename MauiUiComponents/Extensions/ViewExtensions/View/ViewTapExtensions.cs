using System.Runtime.CompilerServices;

namespace MauiUiComponents;

/// <summary>
/// Содержит методы расширения для обработки нажатий
/// на элементы <see cref="View"/> через <see cref="TapGestureRecognizer"/>.
/// </summary>
public static class ViewTapExtensions
{
    #region Fields

    /*
     * Храним только те TapGestureRecognizer, которые были созданы
     * методом ViewOnTapped().
     *
     * ConditionalWeakTable не удерживает View в памяти,
     * поэтому сама таблица не создаёт утечку компонентов.
     */
    private static readonly ConditionalWeakTable<
        View,
        TapGestureRecognizer> _tapRecognizers = new();

    #endregion

    #region Tap

    /// <summary>
    /// Добавляет обработчик одиночного нажатия на элемент.
    /// Повторный вызов для того же элемента заменяет только обработчик,
    /// ранее созданный этим методом, не затрагивая другие GestureRecognizer.
    /// </summary>
    /// <param name="view">
    /// Элемент, для которого регистрируется обработка нажатия.
    /// </param>
    /// <param name="startAction">
    /// Действие, выполняемое непосредственно после нажатия.
    /// </param>
    /// <param name="finishAction">
    /// Дополнительное действие, выполняемое после передачи управления
    /// обратно UI-потоку. Может отсутствовать.
    /// </param>
    /// <returns>
    /// Исходный элемент для продолжения fluent-цепочки.
    /// </returns>
    public static T ViewOnTapped<T>(
        this T view,
        Action<View> startAction,
        Action<View>? finishAction = null)
        where T : View
    {
        ArgumentNullException.ThrowIfNull(view);
        ArgumentNullException.ThrowIfNull(startAction);

        RemoveRegisteredTapRecognizer(
            view);

        var tapRecognizer =
            new TapGestureRecognizer();

        tapRecognizer.Tapped +=
            async (_, _) =>
            {
                startAction(view);

                if (finishAction is null)
                    return;

                /*
                 * Даём MAUI завершить текущий цикл обработки нажатия.
                 * Это особенно полезно для overlay-компонентов:
                 * сначала можно удалить overlay, а затем выполнить
                 * действие, связанное с закрытием или изменением UI.
                 */
                await Task.Yield();

                finishAction(view);
            };

        view.GestureRecognizers.Add(
            tapRecognizer);

        _tapRecognizers.Add(
            view,
            tapRecognizer);

        return view;
    }

    /// <summary>
    /// Удаляет обработчик нажатия, ранее зарегистрированный
    /// методом <see cref="ViewOnTapped{T}(T, Action{View}, Action{View}?)"/>.
    /// Остальные GestureRecognizer элемента не изменяются.
    /// </summary>
    private static void RemoveRegisteredTapRecognizer(
        View view)
    {
        if (!_tapRecognizers.TryGetValue(
                view,
                out var recognizer))
        {
            return;
        }

        view.GestureRecognizers.Remove(
            recognizer);

        _tapRecognizers.Remove(
            view);
    }

    #endregion
}