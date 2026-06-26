namespace MauiUiSettings;

[AttributeUsage(AttributeTargets.Field)]
public sealed class LanguageCodeAttribute : Attribute
{
    public string Code { get; }

    public LanguageCodeAttribute(string code)
    {
        Code = code;
    }
}
