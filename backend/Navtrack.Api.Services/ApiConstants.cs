namespace Navtrack.Api.Services;

public static class ApiConstants
{
    public const string SignalRHubUrlPrefix = "/signalr/";
    public static string HubUrl(string hub) => $"{SignalRHubUrlPrefix}{hub}";
    public const int MaxPasswordResetIn24Hours = 10;
    public const int PasswordResetLinkExpirationHours = 3;

    public static string ResetPasswordLink(string appUrl, string hash) =>
        $"{appUrl}/resetpassword/{hash}";
}