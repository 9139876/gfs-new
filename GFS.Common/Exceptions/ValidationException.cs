using GFS.Common.Errors;

namespace GFS.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationErrors Errors { get; } = new();

    public ValidationException(string field, string message)
    {
        AddError(new ValidationError(field, message));
    }

    public ValidationException(string message)
    {
        AddError(new ValidationError(message));
    }

    public ValidationException(IEnumerable<ValidationError> errors)
    {
        AddErrors(errors);
    }

    public ValidationException(ValidationError error)
    {
        AddError(error);
    }

    private void AddErrors(IEnumerable<ValidationError> errors)
    {
        foreach (var error in errors)
            AddError(error);
    }

    private void AddError(ValidationError error)
    {
        Errors.Add(error);
    }

    public override string Message
        => $"Validation errors: {string.Join("; ", Errors.Select(e => $"{e.Field} - {e.Message}"))}";
}