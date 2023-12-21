using Navtrack.Api.Model.Errors;

namespace Navtrack.Api.Services.Exceptions;

public class ValidationApiException(ApiError apiError) : ApiException(apiError)
{
    public ValidationApiException() : this(ApiErrorCodes.Validation)
    {
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

    public void ThrowIfInvalid()
    {
        if (HasValidationErrors)
        {
            throw this;
        }
    }

    public bool HasValidationErrors => ValidationErrors.Count != 0;
}