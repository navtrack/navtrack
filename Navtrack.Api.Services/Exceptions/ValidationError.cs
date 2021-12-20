namespace Navtrack.Api.Services.Exceptions;

public class ValidationError
{
    public string PropertyName { get; set; }
    public string Code { get; set; }
    public string Message { get; set; }
}