namespace Data.Exceptions;

public class UserCreatingException : Exception
{
    public UserCreatingException() : base()
    {
        
    }

    public UserCreatingException(string message) : base(message)
    {
        
    }
}