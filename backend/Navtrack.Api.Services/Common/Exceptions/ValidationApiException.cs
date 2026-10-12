using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Navtrack.Api.Model.Errors;

namespace Navtrack.Api.Services.Common.Exceptions;

public class ValidationApiException : ApiException
{
    public readonly List<ValidationError> ValidationErrors = [];
    
    public ValidationApiException() : base(ApiErrorCodes.Validation_Generic)
    {
    }
    
    public ValidationApiException(string propertyName, ApiError apiError) : base(ApiErrorCodes.Validation_Generic)
    {
        AddValidationError(propertyName, apiError);
    }
    
    public ValidationApiException AddValidationError(string propertyName, ApiError apiError)
    {
        ValidationErrors.Add(new ValidationError
        {
            PropertyName = propertyName,
            Code = apiError.Code
        });

        return this;
    }
    
    public ValidationApiException AddErrorIfTrue(bool? check, string propertyName, ApiError apiError)
    {
        if (check.GetValueOrDefault())
        {
            ValidationErrors.Add(new ValidationError
            {
                PropertyName = propertyName,
                Code = apiError.Code
            });
        }
      
        return this;
    }
    
    public ValidationApiException AddErrorIfNull([NotNull]object? @object, string propertyName, ApiError apiError)
    {
        if (@object == null)
        {
            ValidationErrors.Add(new ValidationError
            {
                PropertyName = propertyName,
                Code = apiError.Code
            });
        }
      
        return this;
    }
    
    public void ThrowIfInvalid()
    {
        if (ValidationErrors.Count != 0)
        {
            throw this;
        }
    }
}