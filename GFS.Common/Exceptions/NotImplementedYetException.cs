namespace GFS.Common.Exceptions;

public class NotImplementedYetException : NotImplementedException
{
    public NotImplementedYetException() : base("Not implemented yet")
    {
    }

    public NotImplementedYetException(string message) : base(message)
    {
    }
}