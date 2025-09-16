namespace OptionalLibrary;

public class Optional<T>
{
    private readonly T? _value;
    private readonly bool _hasValue;

    private Optional()
    {
        _value =  default(T);
        _hasValue = false;
    }
    
    private Optional(T value)
    {
        _value = value;
        _hasValue = true;
    }

    public T? Value()
    {
        return _value;
    }

    public bool IsPresent()
    {
        return _hasValue;
    }

    public bool IsEmpty()
    {
        return !_hasValue;
    }
    
    public static Optional<T> Empty => new();
    public static Optional<T> Of(T value) => new(value);
}
