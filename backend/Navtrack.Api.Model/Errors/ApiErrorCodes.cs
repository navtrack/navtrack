namespace Navtrack.Api.Model.Errors;

public abstract class ApiErrorCodes
{
    public static readonly ApiError Validation_000001_Generic = new("Validation", "000001", "Validation error.");
    public static readonly ApiError Validation_000002_InvalidCaptcha = new("Validation", "000002", "Invalid captcha.");
   
    public static readonly ApiError Login_000001_InvalidPassword = new("Login", "000001", "Invalid password.");
    public static readonly ApiError Login_000002_SocialLoginNotLinked = new("Login", "000002", "The social login is not linked to the user.");
    public static readonly ApiError Login_000003_InvalidToken = new("Login", "000003", "Invalid token.");

    public static readonly ApiError Asset_000001_UserAlreadyHasRole = new("Asset", "000001", "This user already has a role on this asset.");
    public static readonly ApiError Asset_000002_NameAlreadyUsed = new("Asset", "000002", "You already have an asset with this name.");

    public static readonly ApiError Device_000001_DeviceTypeInvalid = new("Device", "000001", "The device type is invalid.");
    public static readonly ApiError Device_000002_SerialNumberUsed = new("Device", "000002", "The serial number is already used by another device.");
    public static readonly ApiError Device_000003_DeviceIsActive = new("Device", "000003", "A device that is active cannot be deleted.");
    public static readonly ApiError Device_000004_DeviceHasMessages = new("Device", "000004", "A device that has locations cannot be deleted.");

    public static readonly ApiError User_000001_EmailNotFound = new("User", "000001", "There is no user with the provided email.");
    public static readonly ApiError User_000002_EmailAlreadyUsed = new("User", "000002", "The email address is already used.");
    public static readonly ApiError User_000003_PasswordsNotEqual = new("User", "000003", "The passwords do not match.");
    public static readonly ApiError User_000004_InvalidUsernameOrPassword = new("User", "000004", "Invalid username or password.");
    public static readonly ApiError User_000005_MaxPasswordResetsExceeded = new("User", "000005", "Maxim amount of password resets exceeded.");
    public static readonly ApiError User_000006_InvalidPasswordResetHash = new("User", "000006", "The password reset hash provided is invalid.");
    public static readonly ApiError User_000007_ExpiredPasswordResetHash = new("User", "000007", "The password reset hash provided is expired.");
    public static readonly ApiError User_000008_InvalidCurrentPassword = new("User", "000008", "The current password is not valid.");
    public static readonly ApiError User_000009_PasswordMustBeDifferent = new("User", "000009", "Your new password must be different than the current one.");
    
    public static readonly ApiError Organization_000001_UserAlreadyInOrganization = new("Organization", "000001", "The user is already part of the organization.");
    public static readonly ApiError Organization_000002_OneOwnerRequired = new("Organization", "000002", "There must be at least one owner on an organization.");
  
    public static readonly ApiError Team_000001_NameIsUsed = new("Team", "000001", "The team name is already used.");
    public static readonly ApiError Team_000002_TeamAndAssetNotInSameOrganization = new("Team", "000002", "The team and asset do not belong to the same organization.");
    public static readonly ApiError Team_000003_AssetAlreadyInTeam = new("Team", "000003", "This asset is already part of the team.");
    public static readonly ApiError Team_000004_Unused = new("Team", "000004", "UNUSED");
    public static readonly ApiError Team_000005_AssetNotInTeam = new("Team", "000005", "This asset is not part of the team.");
    public static readonly ApiError Team_000006_UserNotInSameOrganization = new("Team", "000006", "The user is not in the same organization.");
    public static readonly ApiError Team_000007_UserAlreadyInTeam = new("Team", "000007", "The user is already part of the team.");
}