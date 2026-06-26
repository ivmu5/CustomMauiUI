using SQLiteStorage;

namespace MauiUiSettings;

public class ColorDictionary : BaseEntity<ColorDictionary>
{
    public const string PrimaryLightArgb = "#FFEC6E4A";
    public const string SecondaryLightArgb = "#FF61C3DF";
    public const string TertiaryLightArgb = "#FF555964";
    public const string TextLightArgb = "#FF000000";
    public const string BackgroundLightArgb = "#FFFFFFFF";
    public const string BlurLightArgb = "#D0FFFFFF"; // "#30FFFFFF"

    public const string PrimaryDarkArgb = "#FFB54F34";
    public const string SecondaryDarkArgb = "#FF3395A9";
    public const string TertiaryDarkArgb = "#FF3A3D48";
    public const string TextDarkArgb = "#FFFFFFFF";
    public const string BackgroundDarkArgb = "#FF000000";
    public const string BlurDarkArgb = "#E0000000"; // "#40000000"

    //Background	#16181D
    //Surface	#1E222A
    //Elevated	#262B35

    public string PrimaryLight { get; set; } = PrimaryLightArgb;
    public string SecondaryLight { get; set; } = SecondaryLightArgb;
    public string TertiaryLight { get; set; } = TertiaryLightArgb;
    public string TextLight { get; set; } = TextLightArgb;
    public string BackgroundLight { get; set; } = BackgroundLightArgb;
    public string BlurLight { get; set; } = BlurLightArgb;

    public string PrimaryDark { get; set; } = PrimaryDarkArgb;
    public string SecondaryDark { get; set; } = SecondaryDarkArgb;
    public string TertiaryDark { get; set; } = TertiaryDarkArgb;
    public string TextDark { get; set; } = TextDarkArgb;
    public string BackgroundDark { get; set; } = BackgroundDarkArgb;
    public string BlurDark { get; set; } = BlurDarkArgb;

    //public static ColorDictionary Default()
    //{
    //    return new ColorDictionary()
    //    {
    //        PrimaryLight = PrimaryLightArgb,
    //        SecondaryLight = SecondaryLightArgb,
    //        TertiaryLight = TertiaryLightArgb,
    //        TextLight = TextLightArgb,
    //        BackgroundLight = BackgroundLightArgb,

    //        PrimaryDark = PrimaryDarkArgb,
    //        SecondaryDark = SecondaryDarkArgb,
    //        TertiaryDark = TertiaryDarkArgb,
    //        TextDark = TextDarkArgb,
    //        BackgroundDark = BackgroundDarkArgb
    //    };
    //}
}
