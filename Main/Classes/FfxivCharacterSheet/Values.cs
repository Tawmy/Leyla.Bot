using SixLabors.ImageSharp.PixelFormats;

namespace Main.Classes.FfxivCharacterSheet;

internal static class Values
{
    public static int FontSizeGrandCompany => 28;
    public static int DimensionsGcFcCrest => 52;
    public static int GcCrestPadding => 12;
    public static Rgba32 ActiveJobLevelBackground => new(0.2f, 0.2f, 0.2f);
    public static int ActiveJobLevelThickness => 40;
    public static int ActiveJobLevelRadius => 10;
    public static int ActiveJobLevelFontSize => 30;
    public static int FontSizeAttributes => 28;
    public static string AttributeGapSmall => "   ";
    public static string AttributeGapBig => "          ";
    public static int FontSizeHomeWorld => 28;
    public static int FontSizeMiMo => 32;
}