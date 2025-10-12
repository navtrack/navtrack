namespace Navtrack.Api;

public static class ApiPaths
{
    public const string Account = "account";
    public const string AccountForgotPassword = "account/forgot-password";
    public const string AccountResetPassword = "account/reset-password";
    
    public const string User = "user";
    public const string UserChangePassword = "user/change-password";
    
    public const string Organizations = "organizations";
    public const string OrganizationById = "organizations/{organizationId}";
    public const string OrganizationAssets = "organizations/{organizationId}/assets";
    public const string OrganizationTeams = "organizations/{organizationId}/teams";
    public const string OrganizationUsers = "organizations/{organizationId}/users";
    public const string OrganizationUserById = "organizations/{organizationId}/users/{userId}";

    public const string AssetById = "assets/{assetId}";
    public const string AssetTrips = "assets/{assetId}/trips";
    public const string AssetUsers = "assets/{assetId}/users";
    public const string AssetUserById = "assets/{assetId}/users/{userId}";
    public const string AssetDevices = "assets/{assetId}/devices";
    public const string AssetDeviceById = "assets/{assetId}/devices/{deviceId}";
    public const string AssetMessages = "assets/{assetId}/messages";
    public const string AssetStats = "assets/{assetId}/stats";
    public const string AssetReportsDistance = "assets/{assetId}/reports/distance";
    public const string AssetReportsFuelConsumption = "assets/{assetId}/reports/fuel-consumption";
    public const string AssetReportsTrips = "assets/{assetId}/reports/trips";
    
    public const string TeamById = "teams/{teamId}";
    public const string TeamUsers = "teams/{teamId}/users";
    public const string TeamUserById = "teams/{teamId}/users/{userId}";
    public const string TeamAssets = "teams/{teamId}/assets";
    public const string TeamAssetById = "teams/{teamId}/assets/{assetId}";
    
    public const string DeviceTypes = "devices/types";
    
    public const string Protocols = "protocols";
    
    public const string GeocodeReverse = "geocode/reverse";
    
    public const string Health = "health";
}