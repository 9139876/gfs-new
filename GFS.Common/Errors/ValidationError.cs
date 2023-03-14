using GFS.Common.Helpers;

namespace GFS.Common.Errors;

public class ValidationError
{
    public string? Field { get; }
    public string? Message { get; }

    //For serialization
    protected ValidationError()
    { }

    public ValidationError(string field, string message)
    {
        Field = field.FirstCharacterToLower();
        Message = message;
    }
    
    public ValidationError(string message)
    {
        Message = message;
    }
}

public class ValidationErrors : List<ValidationError>
{
}