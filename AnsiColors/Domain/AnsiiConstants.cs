namespace AnsiColors.Domain;

public static class AnsiConstants
{
    public const string Prefix = "\\x1b[";
    public const string Escape = "\u001b[";
    public const string Termination = "m";

    // Style
    public const string Reset = Prefix + "0" + Termination;
    public const string Bold = Prefix + "1" + Termination;
    public const string Faint = Prefix + "2" + Termination;
    public const string Italic = Prefix + "3" + Termination;
    public const string Underline = Prefix + "4" + Termination;
    public const string SlowBlink = Prefix + "5" + Termination;
    public const string RapidBlink = Prefix + "6" + Termination;
    public const string Invert = Prefix + "7" + Termination;

    // Rarely Supported
    public const string Hide = Prefix + "8" + Termination;
    public const string CrossedOut = Prefix + "9" + Termination;
    public const string Primary = Prefix + "10" + Termination;
    public const string AltFont1 = Prefix + "11" + Termination;
    public const string AltFont2 = Prefix + "12" + Termination;
    public const string AltFont3 = Prefix + "13" + Termination;
    public const string AltFont4 = Prefix + "14" + Termination;
    public const string AltFont5 = Prefix + "15" + Termination;
    public const string AltFont6 = Prefix + "16" + Termination;
    public const string AltFont7 = Prefix + "17" + Termination;
    public const string AltFont8 = Prefix + "18" + Termination;
    public const string AltFont9 = Prefix + "19" + Termination;
    public const string Fraktur = Prefix + "20" + Termination;
    public const string DoubleUnderline = Prefix + "21" + Termination;
    public const string NormalIntensity = Prefix + "22" + Termination;
    public const string NotItalic = Prefix + "23" + Termination;
    public const string NotUnderlined = Prefix + "24" + Termination;
    public const string NotBlinking = Prefix + "25" + Termination;
    public const string ProportionalSpacing = Prefix + "26" + Termination;
    public const string NotReversed = Prefix + "27" + Termination;
    public const string Reveal = Prefix + "28" + Termination;
    public const string NotCrossedOut = Prefix + "29" + Termination;

    // Foreground Colors
    public const string ForegroundBlack = Prefix + "30" + Termination;
    public const string ForegroundRed = Prefix + "31" + Termination;
    public const string ForegroundGreen = Prefix + "32" + Termination;
    public const string ForegroundYellow = Prefix + "33" + Termination;
    public const string ForegroundBlue = Prefix + "34" + Termination;
    public const string ForegroundMagenta = Prefix + "35" + Termination;
    public const string ForegroundCyan = Prefix + "36" + Termination;
    public const string ForegroundWhite = Prefix + "37" + Termination;
    public const string ForegroundCustom = Prefix + "38" + Termination;
    public const string ForegroundDefault = Prefix + "39" + Termination;

    // BackgroundColors
    public const string BackgroundBlack = Prefix + "40" + Termination;
    public const string BackgroundRed = Prefix + "41" + Termination;
    public const string BackgroundGreen = Prefix + "42" + Termination;
    public const string BackgroundYellow = Prefix + "43" + Termination;
    public const string BackgroundBlue = Prefix + "44" + Termination;
    public const string BackgroundMagenta = Prefix + "45" + Termination;
    public const string BackgroundCyan = Prefix + "46" + Termination;
    public const string BackgroundWhite = Prefix + "47" + Termination;
    public const string BackgroundCustom = Prefix + "48" + Termination;
    public const string BackgroundDefault = Prefix + "49" + Termination;

    // AdditionalStyling
    public const string DisableProportionalSpacing = Prefix + "50" + Termination;
    public const string Framed = Prefix + "51" + Termination;
    public const string Encircled = Prefix + "52" + Termination;
    public const string Overlined = Prefix + "53" + Termination;
    public const string NotFramedEncircled = Prefix + "54" + Termination;
    public const string NotOverlined = Prefix + "55" + Termination;
    public const string Missing56 = Prefix + "56" + Termination;
    public const string Missing57 = Prefix + "57" + Termination;
    public const string UnderlineCustom = Prefix + "58" + Termination;
    public const string UnderlineDefault = Prefix + "59" + Termination;
    public const string IdeogramUnderline = Prefix + "60" + Termination;
    public const string IdeogramDoubleUnderline = Prefix + "61" + Termination;
    public const string IdeogramOverline = Prefix + "62" + Termination;
    public const string IdeogramDoubleOverline = Prefix + "63" + Termination;
    public const string IdeogramStress = Prefix + "64" + Termination;
    public const string IdeogramDisabled = Prefix + "65" + Termination;
    public const string Missing66 = Prefix + "66" + Termination;
    public const string Missing67 = Prefix + "67" + Termination;
    public const string Missing68 = Prefix + "68" + Termination;
    public const string Missing69 = Prefix + "69" + Termination;
    public const string Missing70 = Prefix + "70" + Termination;
    public const string Missing71 = Prefix + "71" + Termination;
    public const string Missing72 = Prefix + "72" + Termination;
    public const string Superscript = Prefix + "73" + Termination;
    public const string Subscript = Prefix + "74" + Termination;
    public const string SuperscriptSubscriptDisable = Prefix + "75" + Termination;
    public const string Missing76 = Prefix + "76" + Termination;
    public const string Missing77 = Prefix + "77" + Termination;
    public const string Missing78 = Prefix + "78" + Termination;
    public const string Missing79 = Prefix + "79" + Termination;
    public const string Missing80 = Prefix + "80" + Termination;
    public const string Missing81 = Prefix + "81" + Termination;
    public const string Missing82 = Prefix + "82" + Termination;
    public const string Missing83 = Prefix + "83" + Termination;
    public const string Missing84 = Prefix + "84" + Termination;
    public const string Missing85 = Prefix + "85" + Termination;
    public const string Missing86 = Prefix + "86" + Termination;
    public const string Missing87 = Prefix + "87" + Termination;
    public const string Missing88 = Prefix + "88" + Termination;
    public const string Missing89 = Prefix + "89" + Termination;

    // Foreground Colors
    public const string BrighterForegroundBlack = Prefix + "90" + Termination;
    public const string BrighterForegroundRed = Prefix + "91" + Termination;
    public const string BrighterForegroundGreen = Prefix + "92" + Termination;
    public const string BrighterForegroundYellow = Prefix + "93" + Termination;
    public const string BrighterForegroundBlue = Prefix + "94" + Termination;
    public const string BrighterForegroundMagenta = Prefix + "95" + Termination;
    public const string BrighterForegroundCyan = Prefix + "96" + Termination;
    public const string BrighterForegroundWhite = Prefix + "97" + Termination;
    public const string Missing98 = Prefix + "98" + Termination;
    public const string Missing99 = Prefix + "99" + Termination;

    // BackgroundColors
    public const string BrighterBackgroundBlack = Prefix + "100" + Termination;
    public const string BrighterBackgroundRed = Prefix + "101" + Termination;
    public const string BrighterBackgroundGreen = Prefix + "102" + Termination;
    public const string BrighterBackgroundYellow = Prefix + "103" + Termination;
    public const string BrighterBackgroundBlue = Prefix + "104" + Termination;
    public const string BrighterBackgroundMagenta = Prefix + "105" + Termination;
    public const string BrighterBackgroundCyan = Prefix + "106" + Termination;
    public const string BrighterBackgroundWhite = Prefix + "107" + Termination;
}