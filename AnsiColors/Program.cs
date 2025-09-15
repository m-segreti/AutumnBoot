using AnsiColors.Domain;

namespace AnsiColors;

public static class Program
{
    public static void Main()
    {
        DisplayAll();
    }

    private static void DisplayAll()
    {
        const string message = "Example Text";
        
        DisplayCodes(message);
        Display8BitColors(AnsiType.Foreground, message);
        Display8BitColors(AnsiType.Background, message);
        
        // These display too much to fit in a console.
        // Display24BitColors(AnsiType.Foreground, message);
        // Display24BitColors(AnsiType.Background, message);

        Display(message, "#650864");
        Display(message, "#ea4335");
    }

    private static void Display(string message, string hex)
    {
        Console.WriteLine(Ansi.Colorize(message, hex));
    }

    private static void DisplayCodes(string message)
    {
        Console.WriteLine("{0,-3} {1,-30} {2,-12}", "ID", "Name", "Example");
        Console.WriteLine("------------------------------------------------------");
        
        for (int i = 0; i < Ansi.Codes.Count; i++)
        {
            string ansiiCode = Ansi.Codes[i].Value;
            string display = Ansi.Format(ansiiCode + message + AnsiConstants.Reset);
            
            Console.WriteLine("{0,-3} {1,-30} {2,-12}", i, Ansi.Codes[i].Key, display);
        }
    }

    private static void Display8BitColors(AnsiType type, string message)
    {
        Console.WriteLine($"\n\n8-Bit Color Codes ({type.ToString()})");
        Console.WriteLine("{0,-3} {1,-13} {2,-7} {3,-12}", "ID", "RGB", "Hex", "Example");
        Console.WriteLine("------------------------------------");

        for (int i = 0; i < 256; i++)
        {
            string display = Ansi.Format(Ansi.Color8Bit(type, i) + message + AnsiConstants.Reset);
            Console.WriteLine("{0,-3} {1,-13} {2,-7} {3,-12}", i, Ansi.Color8BitToRgb(i), Ansi.Color8BitToHex(i), display);
        }
    }
    
    private static void Display24BitColors(AnsiType type, string message)
    {
        Console.WriteLine($"\n\n24-Bit Color Codes ({type.ToString()})");
        Console.WriteLine("{0,-13} {1,-7} {2,-12}", "RGB", "Hex", "Example");
        Console.WriteLine("------------------------------------");

        for (int r = 0; r < 256; r++)
        {
            for (int g = 0; g < 256; g++)
            {
                for (int b = 0; b < 256; b++)
                {
                    string display = Ansi.Format(Ansi.Color24Bit(type, r, g, b) + message + AnsiConstants.Reset);
                    Console.WriteLine("{0,-13} {1,-7} {2,-12}", $"({r},{g},{b})", Ansi.RgbToHex(r, g, b), display);
                }
            }
        }
    }
}
