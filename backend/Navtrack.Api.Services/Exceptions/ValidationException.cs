using Navtrack.Api.Model.Errors;

namespace Navtrack.Api.Services.Exceptions;

public class ValidationException(ApiError apiError) : ApiException(apiError)
{
    public ValidationException() : this(ApiErrorCodes.Validation)
    {
    }
}