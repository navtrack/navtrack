namespace Navtrack.Api.Model.Errors;

public class ApiError(string code)
{
    public readonly string Code = code;
}
