namespace Navtrack.Api.Model.Errors;

public static class ValidationErrorCodes
{
    public static readonly ApiError EmailDoesNotExist = new ("200001", "Email does not exist.");
    public static readonly ApiError PasswordsDoNotMatch = new ("200002", "The passwords do not match.");
    public static readonly ApiError PasswordMustBeNew = new ("200003", "Your new password must be different than the current one.");
    public static readonly ApiError CurrentPasswordInvalid = new ("200004", "The current password is not valid.");
    public static readonly ApiError EmailAlreadyExists = new ("200005", "The email already exists.");
}