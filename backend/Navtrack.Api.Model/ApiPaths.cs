namespace Navtrack.Api.Model;

public static class ApiPaths
{
    public const string Account = "account";
    public const string AccountPasswordForgot = "user/password/forgot";
    public const string AccountPasswordChange = "user/password/change";
    public const string AccountPasswordReset = "user/password/reset";
    public const string User = "user";
    public const string Assets = "assets";
    public const string AssetsAsset = "assets/{assetId}";
    public const string AssetsAssetTrips = "assets/{assetId}/trips";
    public const string AssetsAssetUsers = "assets/{assetId}/users";
    public const string AssetsAssetUsersUser = "assets/{assetId}/users/{userId}";
    public const string AssetsAssetDevices = "assets/{assetId}/devices";
    public const string AssetsAssetDevicesDevice = "assets/{assetId}/devices/{deviceId}";
    public const string AssetsAssetLocations = "assets/{assetId}/locations";
    public const string AssetsAssetReportsTimeDistance = "assets/{assetId}/reports/time-distance";
    public const string DevicesTypes = "devices/types";
    public const string Health = "health";
    public const string Protocols = "protocols";
}