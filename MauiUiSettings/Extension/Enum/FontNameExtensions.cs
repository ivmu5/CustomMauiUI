using System.Reflection;

namespace MauiUiSettings;

public static class FontNameExtensions
{
    public static string GetFontName(this MaterialSymbolMode font)
    {
        return font switch
        {
            MaterialSymbolMode.Auto => string.Empty,
            MaterialSymbolMode.Outlined => DefaultFonts.MaterialSymbolOutlined,
            MaterialSymbolMode.Rounded => DefaultFonts.MaterialSymbolRounded,
            MaterialSymbolMode.Sharp => DefaultFonts.MaterialSymbolSharp,
            _ => throw new NotSupportedException($"Font {font} not supported")
        };
    }

    public static string GetFontName(this FontAliases font)
    {
        return font switch
        {
            FontAliases.GardensCM => DefaultFonts.GardensCM,
            _ => throw new NotSupportedException($"Font {font} not supported")
        };
    }
}
