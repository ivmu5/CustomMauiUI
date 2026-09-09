namespace MauiUiComponents;

/// <summary>
/// Определяет компонент, содержащий типизированное значение,
/// которое может участвовать в привязке и синхронизации данных.
/// </summary>
/// <typeparam name="T">
/// Тип хранимого значения.
/// </typeparam>
public interface IBindableValue<T>
{
    /// <summary>
    /// Получает или устанавливает текущее значение компонента.
    /// </summary>
    T BindableValue { get; set; }
}