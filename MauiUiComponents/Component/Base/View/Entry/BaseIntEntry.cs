namespace MauiUiComponents;

public class BaseIntEntry : BaseEntry<int>
{
    public BaseIntEntry()
        : base()
    {
        Keyboard = Keyboard.Numeric;
    }

    public override int Parse(string input)
    {
        return int.TryParse(input, out var value)
            ? value
            : 0;
    }

    public override string Filter(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        var result = new System.Text.StringBuilder();

        foreach (var c in input)
        {
            if (char.IsDigit(c))
            {
                result.Append(c);
            }
            else if (c == '-' && result.Length == 0)
            {
                result.Append(c);
            }
        }

        return result.ToString();
    }

    public override string Format(int value)
    {
        return value.ToString();
    }
}