using System.Globalization;
using System.Reflection;
using AnsiColors.Domain;

namespace AnsiColors;

/// <summary>
/// ANSI color helper for building escape sequences and converting between
/// indices, RGB, and hex for console display.
/// </summary>
/// <remarks>
/// See <see cref="AnsiType"/> for foreground/background selection and
/// <see cref="AnsiConstants"/> for low-level constants used in sequences.
/// </remarks>
public static class Ansi
{
    /// <summary>
    /// Immutable list of all known ANSI console codes discovered via reflection
    /// from <see cref="AnsiConstants"/>.
    /// </summary>
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

            // Ignore these three fields as they are not indexed Ansi values, just helper constants.
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

    /// <summary>
    /// Builds an ANSI 8-bit <c>foreground</c> color escape for the given index.
    /// </summary>
    /// <param name="value">Color index in the range 0–255.</param>
    /// <returns>
    /// The escape sequence string (e.g., <c>"\u001b[38;5;196m"</c>) for valid values,
    /// otherwise a fallback to <see cref="AnsiConstants.ForegroundWhite"/>.
    /// </returns>
    /// <remarks>
    /// This is a convenience overload that assumes <see cref="AnsiType.Foreground"/>.
    /// Use <see cref="Color8Bit(AnsiType,int)"/> to specify foreground vs background.
    /// </remarks>
    /// <example>
    /// <code>
    /// string color = Ansi.Color8Bit(196); // bright red foreground
    /// </code>
    /// </example>
    public static string Color8Bit(int value)
    {
        return Color8Bit(AnsiType.Foreground, value);
    }

    /// <summary>
    /// Builds an ANSI 8-bit color escape for the given index and type.
    /// </summary>
    /// <param name="type">Foreground or background (<see cref="AnsiType"/>).</param>
    /// <param name="value">Color index in the range 0–255.</param>
    /// <returns>
    /// The escape sequence string (e.g., <c>"\u001b[48;5;21m"</c> for background),
    /// or a white fallback (<see cref="AnsiConstants.ForegroundWhite"/> / <see cref="AnsiConstants.BackgroundWhite"/>)
    /// if <paramref name="value"/> is out of range.
    /// </returns>
    /// <example>
    /// <code>
    /// string color = Ansi.Color8Bit(AnsiType.Foreground, 196); // bright red foreground
    /// </code>
    /// </example>
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

    /// <summary>
    /// Converts an 8-bit index (0–255) to an RGB string <c>"(r,g,b)"</c>.
    /// </summary>
    /// <param name="value">Color index in the range 0–255.</param>
    /// <returns>
    /// <c>"(r,g,b)"</c> for valid values. For out-of-range input, returns <c>"(r,g,b)"</c> placeholder.
    /// </returns>
    /// <remarks>
    /// Indices 0–15 map to the system colors; 16–231 map to the 6×6×6 cube;
    /// 232–255 map to the grayscale ramp.
    /// </remarks>
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

    /// <summary>
    /// Converts an 8-bit index (0–255) to a hex color string <c>#RRGGBB</c>.
    /// </summary>
    /// <param name="value">Color index in the range 0–255.</param>
    /// <returns>A hex string like <c>#FF00AA</c>, or the placeholder <c>#RRGGBB</c> format if out of range.</returns>
    public static string Color8BitToHex(int value)
    {
        return RgbToHex(Color8BitToRgb(value));
    }
    
    /// <summary>
    /// Converts an RGB string in the form <c>"(r,g,b)"</c> to a hex color string <c>#RRGGBB</c>.
    /// </summary>
    /// <param name="rgb">A string like <c>"(255,0,0)"</c> or already a hex color (which is returned unchanged).</param>
    /// <returns>A hex color string <c>#RRGGBB</c>, or the original string if it does not look like <c>"(r,g,b)"</c>.</returns>
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

    /// <summary>
    /// Converts RGB components to a hex color string <c>#RRGGBB</c>.
    /// </summary>
    /// <param name="r">Red (0–255).</param>
    /// <param name="g">Green (0–255).</param>
    /// <param name="b">Blue (0–255).</param>
    /// <returns>Hex color string <c>#RRGGBB</c>.</returns>
    public static string RgbToHex(int r, int g, int b)
    {
        return "#" + ToHex(r) + ToHex(g) + ToHex(b);
    }

    private static string ToHex(int value)
    {
        int bits = value & 0xFF;
        return bits.ToString("X2", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Parses a hex color into RGB components.
    /// </summary>
    /// <param name="hex">Accepts <c>#RRGGBB</c>, <c>RRGGBB</c>, <c>#RGB</c>, <c>RGB</c>, and <c>#AARRGGBB</c>.</param>
    /// <returns>A tuple of <c>(R,G,B)</c> components.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="hex"/> is <c>null</c>.</exception>
    /// <exception cref="FormatException">Thrown if the string is not a supported hex color format.</exception>
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

    /// <summary>
    /// Builds a 24-bit (truecolor) ANSI escape sequence for the provided RGB components.
    /// </summary>
    /// <param name="type">Foreground or background (<see cref="AnsiType"/>).</param>
    /// <param name="r">Red (0–255).</param>
    /// <param name="g">Green (0–255).</param>
    /// <param name="b">Blue (0–255).</param>
    /// <returns>The escape sequence string (e.g., <c>"\u001b[38;2;255;0;0m"</c>).</returns>
    /// <remarks>Values are clamped to 0–255.</remarks>
    public static string Color24Bit(AnsiType type, int r, int g, int b)
    {
        int red = Math.Clamp(r, 0, 255);
        int green = Math.Clamp(g, 0, 255);
        int blue = Math.Clamp(b, 0, 255);
        
        string selector = type == AnsiType.Foreground ? "38;2;" : "48;2;";
        string prefix = AnsiConstants.Prefix + selector;

        return $"{prefix}{red};{green};{blue}{AnsiConstants.Termination}";
    }

    /// <summary>
    /// Alias for <see cref="Color24Bit(AnsiType,int,int,int)"/> using RGB components.
    /// </summary>
    /// <param name="type">Foreground or background (<see cref="AnsiType"/>).</param>
    /// <param name="r">Red (0–255).</param>
    /// <param name="g">Green (0–255).</param>
    /// <param name="b">Blue (0–255).</param>
    /// <returns>The escape sequence string.</returns>
    public static string RgbColor(AnsiType type, int r, int g, int b)
    {
        return Color24Bit(type, r, g, b);
    }

    /// <summary>
    /// Builds a 24-bit (truecolor) ANSI escape sequence from a hex color string.
    /// </summary>
    /// <param name="type">Foreground or background (<see cref="AnsiType"/>).</param>
    /// <param name="hex">Hex color (<c>#RRGGBB</c>, <c>RRGGBB</c>, <c>#RGB</c>, <c>RGB</c>, or <c>#AARRGGBB</c>).</param>
    /// <returns>The escape sequence string.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="hex"/> is <c>null</c>.</exception>
    /// <exception cref="FormatException">Thrown if <paramref name="hex"/> is not a supported hex color format.</exception>
    public static string HexColor(AnsiType type, string hex)
    {
        (int r, int g, int b) rgb = HexToRgb(hex);
        return Color24Bit(type, rgb.r, rgb.g, rgb.b);
    }

    /// <summary>
    /// Rewrites the escape introducer to the literal escape character so the string can be printed as-is.
    /// </summary>
    /// <param name="value">A string containing sequences that start with <see cref="AnsiConstants.Prefix"/>.</param>
    /// <returns>
    /// The same string with <see cref="AnsiConstants.Prefix"/> (<c>"\x1b["</c> as text) replaced by
    /// <see cref="AnsiConstants.Escape"/> (the ESC byte + <c>'['</c>).
    /// </returns>
    public static string Format(string value)
    {
        return value.Replace(AnsiConstants.Prefix, AnsiConstants.Escape);
    }

    /// <summary>
    /// Colors a value with a hex foreground color and appends a reset.
    /// </summary>
    /// <param name="value">The text to colorize.</param>
    /// <param name="hex">Hex color (<c>#RRGGBB</c>, <c>RRGGBB</c>, <c>#RGB</c>, <c>RGB</c>, or <c>#AARRGGBB</c>).</param>
    /// <returns>A printable string with the color applied and a trailing reset.</returns>
    /// <example>
    /// <code>
    /// string colorizedString = Ansi.Colorize("Hello", "#FF8800");
    /// 
    /// // prints "Hello" in orange, then resets attributes
    /// ConsoleWriter.WriteLine(colorizedString);
    /// </code>
    /// </example>
    public static string Colorize(string value, string hex)
    {
        return Format(HexColor(AnsiType.Foreground, hex) + value + AnsiConstants.Reset);
    }
}