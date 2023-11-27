namespace Navtrack.Api.Model;

public static class ApiPaths
{
    public const string User = "user";
    public const string UserPasswordForgot = "user/password/forgot";
    public const string UserPasswordChange = "user/password/change";
    public const string UserPasswordReset = "user/password/reset";
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