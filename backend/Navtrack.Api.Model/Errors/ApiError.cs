namespace Navtrack.Api.Model.Errors;

public class ApiError(string group, string code, string message)
{
    public readonly string Code = $"{group}_{code}".ToUpperInvariant();
    public readonly string Message = message;
}