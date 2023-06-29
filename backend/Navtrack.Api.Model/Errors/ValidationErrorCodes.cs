namespace Navtrack.Api.Model.Errors;

public static class ValidationErrorCodes
{
    public static readonly ApiError EmailDoesNotExist = new ("200001", "Email does not exist.");
    public static readonly ApiError PasswordsDoNotMatch = new ("200002", "The passwords do not match.");
    public static readonly ApiError PasswordMustBeNew = new ("200003", "Your new password must be different than the current one.");
    public static readonly ApiError CurrentPasswordInvalid = new ("200004", "The current password is not valid.");
    public static readonly ApiError EmailAlreadyUsed = new ("200005", "The email address is already used.");
    public static readonly ApiError DeviceTypeInvalid = new ("200007", "The device type is invalid.");
    public static readonly ApiError AssetNameAlreadyUsed = new ("200008", "You already have an asset with this name.");
    public static readonly ApiError SerialNumberAlreadyUsed = new ("200009", "The serial number is already used by another device.");
    public static readonly ApiError DeviceIsActive = new ("200010", "A device that is active cannot be deleted.");
    public static readonly ApiError DeviceHasLocations = new ("200011", "A device that has locations cannot be deleted.");
}