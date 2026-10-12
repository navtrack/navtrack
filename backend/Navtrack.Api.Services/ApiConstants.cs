namespace Navtrack.Api.Services;

public static class ApiConstants
{
    public const int MaxPasswordResetIn24Hours = 10;
    public const int PasswordResetLinkExpirationHours = 3;

    public static string ResetPasswordLink(string appUrl, string id) =>
        $"{appUrl}/resetpassword/{id}";
}