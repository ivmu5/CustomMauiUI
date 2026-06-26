namespace MauiUiComponents;

public class BaseDoubleEntry : BaseEntry<double>
{
    public BaseDoubleEntry()
        : base()
    {
        Keyboard = Keyboard.Numeric;
    }

    public override double Parse(string input)
    {
        return double.TryParse(input, out var value)
            ? value
            : 0;
    }

    public override string Filter(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        var result = new System.Text.StringBuilder();

        bool hasDot = false;

        foreach (var c in input)
        {
            if (char.IsDigit(c))
            {
                result.Append(c);
            }
            else if (c == '.' && !hasDot)
            {
                result.Append(c);
                hasDot = true;
            }
            else if (c == '-' && result.Length == 0)
            {
                result.Append(c);
            }
        }

        return result.ToString();
    }

    public override string Format(double value)
    {
        return value.ToString();
    }
}