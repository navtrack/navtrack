namespace Navtrack.Api.Services.Common.Exceptions;

public class ValidationError
{
    public string PropertyName { get; set; }
    public string Code { get; set; }
}