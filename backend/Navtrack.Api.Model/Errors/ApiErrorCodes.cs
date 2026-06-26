namespace Navtrack.Api.Model.Errors;

public abstract class ApiErrorCodes
{
    public static readonly ApiError Validation_Generic = new("validation.generic");
    public static readonly ApiError Validation_InvalidCaptcha = new("validation.invalid-captcha");
   
    public static readonly ApiError Login_InvalidPassword = new("login.invalid-password");
    public static readonly ApiError Login_SocialLoginNotLinked = new("login.social-login-not-linked");
    public static readonly ApiError Login_InvalidToken = new("login.invalid-token");

    public static readonly ApiError Asset_UserAlreadyHasRole = new("asset.user-already-has-role");
    public static readonly ApiError Asset_NameAlreadyUsed = new("asset.name-already-used");

    public static readonly ApiError Device_DeviceTypeInvalid = new("device.device-type-invalid");
    public static readonly ApiError Device_SerialNumberUsed = new("device.serial-number-used");
    public static readonly ApiError Device_DeviceIsActive = new("device.device-is-active");
    public static readonly ApiError Device_DeviceHasMessages = new("device.device-has-messages");

    public static readonly ApiError User_EmailNotFound = new("user.email-not-found");
    public static readonly ApiError User_EmailAlreadyUsed = new("user.email-already-used");
    public static readonly ApiError User_PasswordsNotEqual = new("user.passwords-not-equal");
    public static readonly ApiError User_InvalidUsernameOrPassword = new("user.invalid-username-or-password");
    public static readonly ApiError User_MaxPasswordResetsExceeded = new("user.max-password-resets-exceeded");
    public static readonly ApiError User_InvalidPasswordResetHash = new("user.invalid-password-reset-hash");
    public static readonly ApiError User_ExpiredPasswordResetHash = new("user.expired-password-reset-hash");
    public static readonly ApiError User_InvalidCurrentPassword = new("user.invalid-current-password");
    public static readonly ApiError User_PasswordMustBeDifferent = new("user.password-must-be-different");
    public static readonly ApiError User_InvalidPassword = new("user.invalid-password");
    public static readonly ApiError User_SoleOrganizationOwner = new("user.sole-organization-owner");
    
    public static readonly ApiError Organization_UserAlreadyInOrganization = new("organization.user-already-in-organization");
    public static readonly ApiError Organization_OneOwnerRequired = new("organization.one-owner-required");
  
    public static readonly ApiError Team_NameIsUsed = new("team.name-is-used");
    public static readonly ApiError Team_TeamAndAssetNotInSameOrganization = new("team.team-and-asset-not-in-same-organization");
    public static readonly ApiError Team_AssetAlreadyInTeam = new("team.asset-already-in-team");
    public static readonly ApiError Team_Unused = new("team.unused");
    public static readonly ApiError Team_AssetNotInTeam = new("team.asset-not-in-team");
    public static readonly ApiError Team_UserNotInSameOrganization = new("team.user-not-in-same-organization");
    public static readonly ApiError Team_UserAlreadyInTeam = new("team.user-already-in-team");
}
