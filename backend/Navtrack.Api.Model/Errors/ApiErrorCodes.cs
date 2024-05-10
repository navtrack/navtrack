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
    public static readonly ApiError EmailDoesNotExist = new ("100007", "Email does not exist.");
    public static readonly ApiError PasswordsDoNotMatch = new ("100008", "The passwords do not match.");
    public static readonly ApiError PasswordMustBeNew = new ("100009", "Your new password must be different than the current one.");
    public static readonly ApiError CurrentPasswordInvalid = new ("100010", "The current password is not valid.");
    public static readonly ApiError EmailAlreadyUsed = new ("100011", "The email address is already used.");
    public static readonly ApiError DeviceTypeInvalid = new ("100012", "The device type is invalid.");
    public static readonly ApiError AssetNameAlreadyUsed = new ("100013", "You already have an asset with this name.");
    public static readonly ApiError SerialNumberAlreadyUsed = new ("100014", "The serial number is already used by another device.");
    public static readonly ApiError DeviceIsActive = new ("100015", "A device that is active cannot be deleted.");
    public static readonly ApiError DeviceHasLocations = new ("100016", "A device that has locations cannot be deleted.");
    public static readonly ApiError NoUserWithEmail = new ("100017", "There is no user with that email.");
    public static readonly ApiError UserAlreadyOnAsset = new ("100018", "This user already has a role on this asset.");
    public static readonly ApiError InvalidRole = new ("100019", "Invalid role.");
}
