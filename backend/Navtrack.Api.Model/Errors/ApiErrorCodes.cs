namespace Navtrack.Api.Model.Errors;

public static class ApiErrorCodes
{
    public static readonly ApiError Validation = new ("100001", "Validation error.");
    public static readonly ApiError RateLimitExceeded = new ("100002", "Rate limit exceeded.");
    public static readonly ApiError InvalidIpAddress = new ("100003", "IP address of the requester is not valid.");
    public static readonly ApiError MaxPasswordResetsExceeded =
        new("100004", "Maxim amount of password resets exceeded.");
    public static readonly ApiError PasswordResetInvalid =
        new("100005", "The password reset hash provided is invalid.");
    public static readonly ApiError PasswordResetExpired =
        new("100006", "The password reset hash provided is expired.");
}