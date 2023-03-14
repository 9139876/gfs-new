using System.ComponentModel.DataAnnotations;
using GFS.Common.Errors;
using ValidationException = GFS.Common.Exceptions.ValidationException;

namespace GFS.Common.Extensions;

public static class ValidationExtensions
{
    public static void Validate(this object? obj)
    {
        if (obj == null)
            throw new NullReferenceException("Validated value is null");
        
        var namesWithAttributes = obj
            .GetType()
            .GetProperties()
            .Select(prop => new KeyValuePair<( string, object ), List<ValidationAttribute>>
            (
                (prop.Name, prop.GetValue(obj))!,
                prop.GetCustomAttributes(true)
                    .Where(attr => attr is ValidationAttribute)
                    .Cast<ValidationAttribute>()
                    .ToList()
            ))
            .Where(pair => pair.Value.Any())
            .ToList();

        if (!namesWithAttributes.Any())
            return;

        var errors = new List<ValidationError>();

        namesWithAttributes.ForEach(pair =>
        {
            var ((name, value), attributes) = pair;
            
            attributes.ForEach(attr =>
            {
                if(!attr.IsValid(value))
                    errors.Add(new ValidationError(name, attr.FormatErrorMessage(name)));
            });
        });

        if (errors.Any())
            throw new ValidationException(errors);
    }
}