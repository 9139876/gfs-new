using System.ComponentModel.DataAnnotations;

namespace GFS.Common.Attributes.Validation;

public class UtcDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
        => value is DateTime { Kind: DateTimeKind.Utc };

    public override string FormatErrorMessage(string name)
        => $"{name} must be DateTime in UTC zone";
}