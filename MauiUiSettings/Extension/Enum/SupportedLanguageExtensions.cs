using System.Reflection;

namespace MauiUiSettings;

public static class SupportedLanguageExtensions
{
    public static string GetCode(this SupportedLanguage language)
    {
        var member = typeof(SupportedLanguage)
            .GetMember(language.ToString())
            .Single();

        return member
            .GetCustomAttribute<LanguageCodeAttribute>()?
            .Code
            ?? throw new InvalidOperationException(
                $"Language code not found for {language}");
    }
}