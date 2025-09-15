namespace AnsiColors.Domain;

/**
 * When dealing with Ansi color codes you are either affecting the foreground (3x) or background (4x).
 */
public enum AnsiType
{
    /**
     * Ansi foreground indicator.
     */
    Foreground,
    
    /**
     * Ansi background indicator.
     */
    Background
}