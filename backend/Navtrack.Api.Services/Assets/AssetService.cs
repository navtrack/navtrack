using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.Devices;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.Extensions;
using Navtrack.Api.Services.Mappers;
using Navtrack.Api.Services.Mappers.Assets;
using Navtrack.Api.Services.User;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Mongo;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.DataAccess.Services.Locations;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IAssetService))]
public class AssetService : IAssetService
{
    private readonly IAssetDataService assetDataService;
    private readonly ICurrentUserAccessor currentUserAccessor;
    private readonly ILocationDataService locationDataService;
    private readonly IDeviceTypeDataService deviceTypeDataService;
    private readonly IDeviceService deviceService;
    private readonly IRepository repository;
    private readonly IUserDataService userDataService;

    public AssetService(IAssetDataService assetDataService, ICurrentUserAccessor currentUserAccessor,
        ILocationDataService locationDataService, IDeviceTypeDataService deviceTypeDataService,
        IDeviceService deviceService, IRepository repository, IUserDataService userDataService)
    {
        this.assetDataService = assetDataService;
        this.currentUserAccessor = currentUserAccessor;
        this.locationDataService = locationDataService;
        this.deviceTypeDataService = deviceTypeDataService;
        this.deviceService = deviceService;
        this.repository = repository;
        this.userDataService = userDataService;
    }

    public async Task<AssetModel> GetById(string assetId)
    {
        AssetDocument asset = await assetDataService.GetById(assetId);
        UserDocument user = await currentUserAccessor.GetCurrentUser();
        DeviceType deviceType = deviceTypeDataService.GetById(asset.Device.DeviceTypeId);

        return AssetModelMapper.Map(asset, user.UnitsType, deviceType);
    }

    public async Task<AssetsModel> GetAssets()
    {
        UserDocument user = await currentUserAccessor.GetCurrentUser();
        List<ObjectId> assetIds = user.AssetRoles?.Select(x => x.AssetId).ToList() ??
                                  Enumerable.Empty<ObjectId>().ToList();
        List<AssetDocument> assets = await assetDataService.GetAssetsByIds(assetIds);

        List<string> assetDeviceTypes =
            assets.Select(x => x.Device.DeviceTypeId).Distinct().ToList();

        IEnumerable<DeviceType> deviceTypes =
            deviceTypeDataService.GetDeviceTypes().Where(x => assetDeviceTypes.Contains(x.Id));

        AssetsModel assetList = AssetListMapper.Map(assets, user.UnitsType, deviceTypes);

        return assetList;
    }

    public async Task Update(string assetId, UpdateAssetModel model)
    {
        AssetModel asset = await GetById(assetId);
        asset.Throw404IfNull();

        if (!string.IsNullOrEmpty(model.Name) && asset.Name != model.Name)
        {
            UserDocument user = await currentUserAccessor.GetCurrentUser();

            bool nameIsUsed = await assetDataService.NameIsUsed(model.Name, user.Id, assetId);

            if (nameIsUsed)
            {
                throw new ApiException()
                    .AddValidationError(nameof(model.Name), "You already have an asset with this name.");
            }

            await assetDataService.UpdateName(assetId, model.Name);
        }
    }

    public async Task Delete(string assetId)
    {
        Task deleteAssetTask = assetDataService.Delete(assetId);
        Task deleteLocationsTask = locationDataService.DeleteByAssetId(assetId);
        Task removeRoleTask = userDataService.DeleteAssetRoles(assetId);

        await Task.WhenAll(new List<Task> { deleteAssetTask, deleteLocationsTask, removeRoleTask });
    }

    public async Task<AssetModel> Add(AddAssetModel model)
    {
        UserDocument user = await currentUserAccessor.GetCurrentUser();

        AdjustModel(model);
        await ValidateModel(model, user);

        AssetDocument assetDocument = await AddDocuments(model);
        DeviceType deviceType = deviceTypeDataService.GetById(model.DeviceTypeId);

        return AssetModelMapper.Map(assetDocument, user.UnitsType, deviceType);
    }

    public async Task<AssetUserListModel> GetAssetUsers(string assetId)
    {
        AssetDocument asset = await assetDataService.GetById(assetId);
        List<UserDocument> users = await userDataService.GetUsersByIds(asset.UserRoles?.Select(x => x.UserId));

        return AssetUsersModelMapper.Map(asset, users);
    }

    public async Task AddUserToAsset(string assetId, AddUserToAssetModel model)
    {
        AssetDocument asset = await assetDataService.GetById(assetId);
        asset.Throw404IfNull();

        UserDocument userDocument = await userDataService.GetUserByEmail(model.Email);
        ApiException validationException = new();

        if (userDocument == null)
        {
            validationException.AddValidationError(nameof(model.Email), "There is no user with that email.");
        }
        else if (asset.UserRoles.Any(x => x.UserId == userDocument.Id))
        {
            validationException.AddValidationError(nameof(model.Email),
                "This user already has a role on this asset.");
        }

        if (!Enum.TryParse(model.Role, out AssetRoleType assetRoleType))
        {
            validationException.AddValidationError(nameof(model.Role), "Invalid role.");
        }

        if (validationException.HasValidationErrors)
        {
            throw validationException;
        }

        await assetDataService.AddUserToAsset(asset, userDocument, assetRoleType);
    }

    public async Task RemoveUserFromAsset(string assetId, string userId)
    {
        AssetDocument asset = await assetDataService.GetById(assetId);
        asset.Throw404IfNull();

        AssetUserRoleElement assetUserRole = asset.UserRoles.FirstOrDefault(x => x.UserId == ObjectId.Parse(userId));

        asset.ThrowApiExceptionIfNull(HttpStatusCode.BadRequest);

        if (assetUserRole!.Role == AssetRoleType.Owner && asset.UserRoles.Count(x => x.Role == AssetRoleType.Owner) < 2)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "You cannot remove the only owner of the asset.");
        }

        await assetDataService.RemoveUserFromAsset(assetId, userId);
    }

    private async Task<AssetDocument> AddDocuments(AddAssetModel model)
    {
        UserDocument currentUser = await currentUserAccessor.GetCurrentUser();
        DeviceType deviceType = deviceTypeDataService.GetById(model.DeviceTypeId);

        AssetDocument assetDocument = AssetDocumentMapper.Map(model, currentUser);
        await repository.GetCollection<AssetDocument>().InsertOneAsync(assetDocument);

        DeviceDocument deviceDocument = DeviceDocumentMapper.Map(model, assetDocument.Id, currentUser.Id);
        await repository.GetCollection<DeviceDocument>().InsertOneAsync(deviceDocument);

        assetDocument.Device = ActiveDeviceElementMapper.Map(deviceDocument, deviceType);
        
        await repository.GetCollection<UserDocument>().UpdateOneAsync(x => x.Id == currentUser.Id,
            Builders<UserDocument>.Update.AddToSet(x => x.AssetRoles, new UserAssetRoleElement
            {
                Id = ObjectId.GenerateNewId(),
                Role = AssetRoleType.Owner,
                AssetId = assetDocument.Id
            }));

        await assetDataService.SetActiveDevice(assetDocument.Id, deviceDocument.Id, deviceDocument.SerialNumber,
            deviceDocument.DeviceTypeId, deviceType.Protocol.Port);

        return assetDocument;
    }

    private async Task ValidateModel(AddAssetModel model, UserDocument currentUser)
    {
        ApiException validationException = new();

        if (!deviceTypeDataService.Exists(model.DeviceTypeId))
        {
            validationException.AddValidationError(nameof(model.DeviceTypeId), "The device type is not valid.");
        }

        if (await assetDataService.NameIsUsed(model.Name, currentUser.Id))
        {
            validationException.AddValidationError(nameof(model.Name),
                "You already have an asset with this name.");
        }

        if (await deviceService.SerialNumberIsUsed(model.SerialNumber, model.DeviceTypeId))
        {
            validationException.AddValidationError(nameof(model.SerialNumber),
                "The serial number is already used by another device.");
        }

        if (validationException.HasValidationErrors)
        {
            throw validationException;
        }
    }

    private static void AdjustModel(AddAssetModel model)
    {
        model.Name = model.Name?.Trim();
    }
}