namespace AnsiColors.Domain;

/**
 * Simple immutable key/value object representation.
 */
public readonly record struct KeyValue(string Key, string Value)
{
    /**
     * Returns a string representation of the key/value pair in [key: value] format.
     */
    public override string ToString()
    {
        return $"{Key}: {Value}";
    }
}