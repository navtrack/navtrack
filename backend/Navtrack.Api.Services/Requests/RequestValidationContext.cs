using Navtrack.Api.Services.Common.Exceptions;

namespace Navtrack.Api.Services.Requests;

public class RequestValidationContext<TRequest>
{
    public ValidationApiException ValidationException { get; set; }
    public TRequest Request { get; set; }
}