using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace MauiUiComponents;

public class LocalizationResourceManager<T> : BindableObject, ILocalizationResourceManager, IDisposable
{
    private readonly LanguageService _languageService;
    private readonly ResourceManager _resourceManager;

    public LocalizationResourceManager(LanguageService languageService)
    {
        _languageService = languageService;

        _resourceManager = typeof(T)
            .GetProperty(nameof(ResourceManager))!
            .GetValue(null) as ResourceManager
            ?? throw new InvalidOperationException("ResourceManager not found");

        _languageService.PropertyChanged += OnLanguageChanged;
    }

    public string this[string key]
        => _resourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? key;

    private void OnLanguageChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(null);
    }

    public void Dispose()
    {
        _languageService.PropertyChanged -= OnLanguageChanged;
    }
}