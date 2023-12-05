namespace Navtrack.Api.Model.Errors;

public class ApiError(string code, string message)
{
    public readonly string Code = code;
    public readonly string Message = message;
}