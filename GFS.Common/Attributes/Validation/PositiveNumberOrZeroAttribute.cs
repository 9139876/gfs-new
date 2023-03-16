using System.ComponentModel.DataAnnotations;

namespace GFS.Common.Attributes.Validation;

public class PositiveNumberOrZeroAttribute: ValidationAttribute
{
    public override bool IsValid(object? value)
        => value is >= 0;

    public override string FormatErrorMessage(string name)
        => $"{name} must be number more or equal zero";
}