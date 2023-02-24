namespace Navtrack.Api.Model.Errors;

public class ApiError
{
    public readonly string Code;
    public readonly string Message;
    
    public ApiError(string code, string message)
    {
        Code = code;
        Message = message;
    }
}