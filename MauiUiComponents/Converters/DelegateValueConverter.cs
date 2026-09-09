using System.Globalization;

namespace MauiUiComponents;

/// <summary>
/// Универсальный конвертер значений для MAUI Binding,
/// использующий переданные делегаты для прямого и обратного преобразования.
/// </summary>
/// <typeparam name="TSource">
/// Тип исходного значения.
/// </typeparam>
/// <typeparam name="TTarget">
/// Тип значения целевого свойства.
/// </typeparam>
public sealed class DelegateValueConverter<TSource, TTarget> : IValueConverter
{
    #region Fields

    private readonly Func<TSource, TTarget>? _convert;
    private readonly Func<TTarget, TSource>? _convertBack;

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт конвертер на основе пользовательских функций преобразования.
    /// Любое из направлений может отсутствовать.
    /// </summary>
    /// <param name="convert">
    /// Функция преобразования значения из исходного типа в целевой.
    /// </param>
    /// <param name="convertBack">
    /// Функция обратного преобразования значения из целевого типа в исходный.
    /// </param>
    public DelegateValueConverter(
        Func<TSource, TTarget>? convert = null,
        Func<TTarget, TSource>? convertBack = null)
    {
        _convert = convert;
        _convertBack = convertBack;
    }

    #endregion

    #region Conversion

    /// <summary>
    /// Преобразует значение из типа источника в тип целевого свойства.
    /// </summary>
    /// <remarks>
    /// Если функция прямого преобразования не задана,
    /// исходное значение возвращается без изменений.
    /// </remarks>
    public object? Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        if (_convert is null)
            return value;

        if (value is not TSource sourceValue)
        {
            throw new InvalidOperationException(
                $"Cannot convert value of type " +
                $"'{value?.GetType().Name ?? "null"}' " +
                $"using converter with source type " +
                $"'{typeof(TSource).Name}'.");
        }

        return _convert(
            sourceValue);
    }

    /// <summary>
    /// Выполняет обратное преобразование значения
    /// из типа целевого свойства в тип источника.
    /// </summary>
    /// <remarks>
    /// Если функция обратного преобразования не задана,
    /// значение возвращается без изменений.
    /// </remarks>
    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        if (_convertBack is null)
            return value;

        if (value is not TTarget targetValue)
        {
            throw new InvalidOperationException(
                $"Cannot convert value of type " +
                $"'{value?.GetType().Name ?? "null"}' " +
                $"using converter with target type " +
                $"'{typeof(TTarget).Name}'.");
        }

        return _convertBack(
            targetValue);
    }

    #endregion
}