using System.Globalization;

namespace MauiUiComponents;

public sealed class DelegateValueConverter<TSource, TTarget> : IValueConverter
{
    private readonly Func<TSource, TTarget>? _convert;
    private readonly Func<TTarget, TSource>? _convertBack;

    public DelegateValueConverter(
        Func<TSource, TTarget>? convert,
        Func<TTarget, TSource>? convertBack)
    {
        _convert = convert;
        _convertBack = convertBack;
    }

    public object? Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        if (_convert is null)
            return value;

        return _convert((TSource)value!);
    }

    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        if (_convertBack is null)
            return value;

        return _convertBack((TTarget)value!);
    }
}
