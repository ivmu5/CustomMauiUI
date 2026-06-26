namespace MauiUiComponents;

public interface ILocalizationResourceManager
{
    string this[string key] { get; }
}
