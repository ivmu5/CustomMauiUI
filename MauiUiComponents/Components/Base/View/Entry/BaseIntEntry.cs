using System.Globalization;

namespace MauiUiComponents;

/// <summary>
/// Типизированное поле ввода для целочисленных значений.
/// </summary>
/// <remarks>
/// Поддерживает ввод цифр и ведущего знака минус,
/// а также использует числовую клавиатуру платформы.
/// </remarks>
public sealed class BaseIntEntry :
    BaseEntry<int>
{
    #region Constructor

    /// <summary>
    /// Создаёт поле ввода целого числа
    /// и настраивает числовую клавиатуру.
    /// </summary>
    public BaseIntEntry()
    {
        Keyboard =
            Keyboard.Numeric;
    }

    #endregion

    #region Conversion

    /// <summary>
    /// Пытается преобразовать текст
    /// в целочисленное значение.
    /// </summary>
    protected override bool Parse(
        string text,
        out int value)
    {
        return int.TryParse(
            text,
            NumberStyles.Integer,
            CultureInfo.CurrentCulture,
            out value);
    }

    /// <summary>
    /// Преобразует целочисленное значение
    /// в текст для отображения.
    /// </summary>
    protected override string Format(
        int value)
    {
        return value.ToString(
            CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Удаляет из пользовательского ввода
    /// все символы, кроме цифр и ведущего знака минус.
    /// </summary>
    protected override string Filter(
        string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        var result =
            new char[text.Length];

        var index =
            0;

        for (var i = 0; i < text.Length; i++)
        {
            var character =
                text[i];

            if (char.IsDigit(character))
            {
                result[index++] =
                    character;

                continue;
            }

            if (character == '-' &&
                i == 0)
            {
                result[index++] =
                    character;
            }
        }

        return new string(
            result,
            0,
            index);
    }

    #endregion
}