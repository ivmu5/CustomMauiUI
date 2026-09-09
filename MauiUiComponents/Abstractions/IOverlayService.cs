namespace MauiUiComponents;

/// <summary>
/// Определяет контракт сервиса для отображения
/// и удаления временных overlay-компонентов.
/// </summary>
public interface IOverlayService
{
    /// <summary>
    /// Добавляет представление поверх основного содержимого страницы.
    /// </summary>
    /// <param name="view">
    /// Представление, которое необходимо показать как overlay.
    /// </param>
    /// <param name="placement">
    /// Способ расположения overlay.
    /// </param>
    /// <param name="anchor">
    /// Элемент, относительно которого располагается overlay.
    /// Для размещения по центру может отсутствовать.
    /// </param>
    /// <param name="onOverlayTapped">
    /// Дополнительное действие, выполняемое после нажатия
    /// на overlay-слой.
    /// </param>
    void AddOverlay(
        View view,
        OverlayPlacement placement,
        View? anchor = null,
        Action<View>? onOverlayTapped = null);

    /// <summary>
    /// Удаляет ранее добавленный overlay.
    /// Если указанное представление не зарегистрировано,
    /// реализация сервиса может проигнорировать вызов.
    /// </summary>
    /// <param name="view">
    /// Представление overlay, которое необходимо удалить.
    /// </param>
    void RemoveOverlay(
        View view);
}