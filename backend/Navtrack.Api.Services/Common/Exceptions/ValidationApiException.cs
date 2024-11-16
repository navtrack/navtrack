using System.Diagnostics.CodeAnalysis;
using Navtrack.Api.Model.Errors;

namespace Navtrack.Api.Services.Common.Exceptions;

public class ValidationApiException : ApiException
{
    public ValidationApiException() : base(ApiErrorCodes.Validation_000001_Generic)
    {
    }
    
    public ValidationApiException(string propertyName, ApiError apiError) : base(ApiErrorCodes.Validation_000001_Generic)
    {
        AddValidationError(propertyName, apiError);
    }
    
    public ApiException AddValidationError(string propertyName, ApiError apiError)
    {
        ValidationErrors.Add(new ValidationError
        {
            PropertyName = propertyName,
            Code = apiError.Code
        });

        return this;
    }
    
    public ApiException AddErrorIfTrue(bool? check, string propertyName, ApiError apiError)
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
    
    public ApiException AddErrorIfNull([NotNull]object? @object, string propertyName, ApiError apiError)
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