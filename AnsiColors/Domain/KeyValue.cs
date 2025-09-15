namespace AnsiColors.Domain;

public readonly record struct KeyValue(string Key, string Value)
{
    public override string ToString()
    {
        return $"{Key}: {Value}";
    }
}