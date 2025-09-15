using System.Globalization;
using System.Reflection;
using AnsiColors.Domain;

namespace AnsiColors;

public static class Ansi
{
    public static IReadOnlyList<KeyValue> Codes { get; }

    static Ansi()
    {
        KeyValue[] items = BuildViaReflection();
        Codes = Array.AsReadOnly(items);
    }

    private static KeyValue[] BuildViaReflection()
    {
        Type type = typeof(AnsiConstants);
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
        List<KeyValue> list = new List<KeyValue>(fields.Length);

        foreach (FieldInfo f in fields)
        {
            if (f.FieldType != typeof(string))
            {
                continue;
            }

            string name = f.Name;

            if (name.Equals("Prefix") || name.Equals("Escape") || name.Equals("Termination"))
            {
                continue;
            }

            string? value = (string?)f.GetValue(null);

            if (value is not null)
            {
                list.Add(new KeyValue(name, value));
            }
        }

        return list.ToArray();
    }

    public static string Color8Bit(int value)
    {
        return Color8Bit(AnsiType.Foreground, value);
    }

    public static string Color8Bit(AnsiType type, int value)
    {
        if (value is < 0 or > 255)
        {
            return type == AnsiType.Foreground
                ? AnsiConstants.ForegroundWhite
                : AnsiConstants.BackgroundWhite;
        }

        string selector = type == AnsiType.Foreground ? "38;5;" : "48;5;";
        string prefix = AnsiConstants.Prefix + selector;

        return prefix + value + AnsiConstants.Termination;
    }

    public static string Color8BitToRgb(int value)
    {
        return value switch
        {
            < 0 or > 255 => "(r,g,b)",
            <= 15 => value switch
            {
                0  => "(0,0,0)",
                1  => "(128,0,0)",
                2  => "(0,128,0)",
                3  => "(128,128,0)",
                4  => "(0,0,128)",
                5  => "(128,0,128)",
                6  => "(0,128,128)",
                7  => "(192,192,192)",
                8  => "(128,128,128)",
                9  => "(255,0,0)",
                10 => "(0,255,0)",
                11 => "(255,255,0)",
                12 => "(0,0,255)",
                13 => "(255,0,255)",
                14 => "(0,255,255)",
                15 => "(255,255,255)",
                _ => "(r,g,b)"
            },
            <= 231 => ToCubeRgb(value),
            _ => ToGrayRgb(value),
        };
    }
    
    private static string ToCubeRgb(int value)
    {
        int i = value - 16;
        int red = i / 36;
        int green = (i % 36) / 6;
        int blue = i % 6;

        int[] steps = [0, 95, 135, 175, 215, 255];

        int r = steps[red];
        int g = steps[green];
        int b = steps[blue];

        return "(" + r + "," + g + "," + b + ")";
    }

    private static string ToGrayRgb(int value)
    {
        int level = 8 + 10 * (value - 232);
        return "(" + level + "," + level + "," + level + ")";
    }

    public static string Color8BitToHex(int value)
    {
        return RgbToHex(Color8BitToRgb(value));
    }
    
    public static string RgbToHex(string rgb)
    {
        if (!rgb.Contains('(') && !rgb.Contains(')'))
        {
            return rgb;
        }
        
        string parsed = rgb.Replace("(", "").Replace(")", "");
        string[] parts = parsed.Split(',');
        int r = int.Parse(parts[0]);
        int g = int.Parse(parts[1]);
        int b = int.Parse(parts[2]);
        
        return RgbToHex(r, g, b);
    }

    public static string RgbToHex(int r, int g, int b)
    {
        return "#" + ToHex(r) + ToHex(g) + ToHex(b);
    }

    private static string ToHex(int value)
    {
        int bits = value & 0xFF;
        return bits.ToString("X2", CultureInfo.InvariantCulture);
    }

    public static (int R, int G, int B) HexToRgb(string hex)
    {
        if (hex is null)
        {
            throw new ArgumentNullException(nameof(hex));
        }

        string normalized = Normalize(hex, out bool hasAlpha);
        int r = 0;
        int g = 0;
        int b = 0;

        if (normalized.Length == 6) // RRGGBB
        {
            r = ParseByte(normalized.Substring(0, 2));
            g = ParseByte(normalized.Substring(2, 2));
            b = ParseByte(normalized.Substring(4, 2));
        }
        else if (normalized.Length == 3) // RGB (short) -> expand
        {
            r = ParseByte(new string(normalized[0], 2));
            g = ParseByte(new string(normalized[1], 2));
            b = ParseByte(new string(normalized[2], 2));
        }
        else if (normalized.Length == 8 && hasAlpha) // AARRGGBB -> ignore A, return RGB
        {
            r = ParseByte(normalized.Substring(2, 2));
            g = ParseByte(normalized.Substring(4, 2));
            b = ParseByte(normalized.Substring(6, 2));
        }

        return (r, g, b);
    }

    private static string Normalize(string hex, out bool hasAlpha)
    {
        string s = hex.Trim();

        if (s.StartsWith("#", StringComparison.Ordinal))
        {
            s = s.Substring(1);
        }
        else if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            s = s.Substring(2);
        }

        hasAlpha = s.Length == 8;
        
        return s;
    }
    
    private static int ParseByte(string twoHexDigits)
    {
        return int.Parse(twoHexDigits, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
    }

    public static string Color24Bit(AnsiType type, int r, int g, int b)
    {
        int red = Math.Clamp(r, 0, 255);
        int green = Math.Clamp(g, 0, 255);
        int blue = Math.Clamp(b, 0, 255);
        
        string selector = type == AnsiType.Foreground ? "38;2;" : "48;2;";
        string prefix = AnsiConstants.Prefix + selector;

        return $"{prefix}{red};{green};{blue}{AnsiConstants.Termination}";
    }

    public static string RgbColor(AnsiType type, int r, int g, int b)
    {
        return Color24Bit(type, r, g, b);
    }

    public static string HexColor(AnsiType type, string hex)
    {
        (int r, int g, int b) rgb = HexToRgb(hex);
        return Color24Bit(type, rgb.r, rgb.g, rgb.b);
    }

    public static string Format(string value)
    {
        return value.Replace(AnsiConstants.Prefix, AnsiConstants.Escape);
    }

    public static string Colorize(string value, string hex)
    {
        return Format(HexColor(AnsiType.Foreground, hex) + value + AnsiConstants.Reset);
    }
}