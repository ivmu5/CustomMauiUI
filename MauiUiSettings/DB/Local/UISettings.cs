using SQLiteStorage;

namespace MauiUiSettings;

public class UISettings : BaseEntity<UISettings>, IInstanceEntity<UISettings>
{
    public bool IsInstance { get; set; }

    public ThemeType ThemeType { get; set; } = ThemeType.System;

    [ForeignKey(nameof(ColorsId))]
    public ColorDictionary Colors { get; set; } = null!;
    public Guid ColorsId { get; set; }


    public double FontSize { get; set; } = TextFontService.DefaultFontSize;
    public string FontFamily { get; set; } = TextFontService.DefaultFontFamily;

    public MaterialSymbolMode IconFamilyMode = IconFontService.DefaultIconFamilyMode;
    public double IconSize = IconFontService.DefaultIconSize;

    public int CornerRadius { get; set; } = CornerRadiusService.DefaultCornerRadius;

    public SupportedLanguage Language { get; set; } = LanguageService.DefaultLanguage;

    public int MaxSnackbarQueueSize { get; set; } = SnackbarService.DefaultMaxQueueSize;
}

