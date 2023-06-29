using Navtrack.Api.Model.Errors;

namespace Navtrack.Api.Services.Exceptions;

public class ValidationException : ApiException
{
    public ValidationException() : base(ApiErrorCodes.Validation)
    {
    }

    public ValidationException(ApiError apiError) : base(apiError)
    {
    }
}