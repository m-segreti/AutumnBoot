namespace Server.Domain;

public class DefaultResponseBuilder
{
    private int _status = 200;
    private string _message = "";

    public static DefaultResponseBuilder Builder()
    {
        return new DefaultResponseBuilder();
    }

    public static DefaultResponse Of(string message)
    {
        return new DefaultResponseBuilder
        {
            _message = message
        }.Build();
    }

    public DefaultResponse Build()
    {
        return new DefaultResponse(_status, _message);
    }
    
    public DefaultResponseBuilder Status(int status) 
    {
        _status = status;
        return this;
    }
    
    public DefaultResponseBuilder Message(string message) 
    {
        _message = message;
        return this;
    }
    
}