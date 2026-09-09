using System.Globalization;

namespace MauiUiComponents;

/// <summary>
/// Типизированное поле ввода для чисел с плавающей точкой.
/// </summary>
/// <remarks>
/// Поддерживает ввод цифр, ведущего знака минус
/// и одного десятичного разделителя.
/// Точка и запятая автоматически приводятся
/// к разделителю текущей культуры.
/// </remarks>
public sealed class BaseDoubleEntry :
    BaseEntry<double>
{
    #region Constructor

    /// <summary>
    /// Создаёт поле ввода числа с плавающей точкой
    /// и настраивает числовую клавиатуру.
    /// </summary>
    public BaseDoubleEntry()
    {
        Keyboard =
            Keyboard.Numeric;
    }

    #endregion

    #region Conversion

    /// <summary>
    /// Пытается преобразовать текст
    /// в значение типа <see cref="double"/>.
    /// </summary>
    protected override bool Parse(
        string text,
        out double value)
    {
        return double.TryParse(
            text,
            NumberStyles.Float,
            CultureInfo.CurrentCulture,
            out value);
    }

    /// <summary>
    /// Преобразует числовое значение
    /// в текст с использованием текущей культуры.
    /// </summary>
    protected override string Format(
        double value)
    {
        return value.ToString(
            CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Удаляет недопустимые символы из пользовательского ввода
    /// и нормализует десятичный разделитель.
    /// </summary>
    protected override string Filter(
        string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        var decimalSeparator =
            CultureInfo.CurrentCulture
                .NumberFormat
                .NumberDecimalSeparator;

        var result =
            new List<char>(
                text.Length);

        var hasDecimalSeparator =
            false;

        for (var i = 0; i < text.Length; i++)
        {
            var character =
                text[i];

            if (char.IsDigit(character))
            {
                result.Add(
                    character);

                continue;
            }

            if (character == '-' &&
                i == 0)
            {
                result.Add(
                    character);

                continue;
            }

            if ((character == '.' ||
                 character == ',') &&
                !hasDecimalSeparator)
            {
                /*
                 * Пользователь может ввести как точку,
                 * так и запятую. Внутри Entry всегда используем
                 * разделитель текущей культуры.
                 */
                result.Add(
                    decimalSeparator[0]);

                hasDecimalSeparator =
                    true;
            }
        }

        return new string(
            result.ToArray());
    }

    #endregion
}