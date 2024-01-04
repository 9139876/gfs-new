using System.ComponentModel.DataAnnotations;

namespace GFS.Common.Attributes.Validation;

public class NullableUtcDateAttribute: ValidationAttribute
{
    public override bool IsValid(object? value)
        => value is null or DateTime { Kind: DateTimeKind.Utc };

    public override string FormatErrorMessage(string name)
        => $"{name} must be DateTime in UTC zone";
}