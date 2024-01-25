using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Devices;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.Extensions;
using Navtrack.Api.Services.Mappers.Assets;
using Navtrack.Api.Services.Mappers.Devices;
using Navtrack.Api.Services.User;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Mongo;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.DataAccess.Services.Positions;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IAssetService))]
public class AssetService(
    IAssetRepository assetRepository,
    ICurrentUserAccessor userAccessor,
    IPositionRepository positionRepository,
    IDeviceTypeRepository deviceTypeRepository,
    IDeviceService service,
    IRepository repository,
    IUserRepository userRepository,
    IPost post)
    : IAssetService
{
    public async Task<AssetModel> GetById(string assetId)
    {
        AssetDocument asset = await assetRepository.GetById(assetId);
        DeviceType deviceType = deviceTypeRepository.GetById(asset.Device.DeviceTypeId);
        List<UserDocument> users = await userRepository.GetUsersByIds(asset.UserRoles.Select(x => x.UserId));

        return AssetModelMapper.Map(asset, deviceType, users);
    }

    public async Task<ListModel<AssetModel>> GetAssets()
    {
        UserDocument user = await userAccessor.Get();
        List<ObjectId> assetIds = user.AssetRoles?.Select(x => x.AssetId).ToList() ??
                                  Enumerable.Empty<ObjectId>().ToList();
        List<AssetDocument> assets = await assetRepository.GetAssetsByIds(assetIds);

        List<string> assetDeviceTypes =
            assets.Select(x => x.Device.DeviceTypeId).Distinct().ToList();

        IEnumerable<DeviceType> deviceTypes =
            deviceTypeRepository.GetDeviceTypes().Where(x => assetDeviceTypes.Contains(x.Id));

        ListModel<AssetModel> model = AssetListMapper.Map(assets, user.UnitsType, deviceTypes);

        return model;
    }

    public async Task Update(string assetId, UpdateAssetModel model)
    {
        AssetModel asset = await GetById(assetId);
        asset.Return404IfNull();

        if (!string.IsNullOrEmpty(model.Name) && asset.Name != model.Name)
        {
            UserDocument user = await userAccessor.Get();

            bool nameIsUsed = await assetRepository.NameIsUsed(model.Name, user.Id, assetId);

            if (nameIsUsed)
            {
                throw new ValidationApiException()
                    .AddValidationError(nameof(model.Name), ValidationErrorCodes.AssetNameAlreadyUsed);
            }

            await assetRepository.UpdateName(assetId, model.Name);
        }
    }

    public async Task Delete(string assetId)
    {
        Task deleteAssetTask = assetRepository.Delete(assetId);
        Task deleteLocationsTask = positionRepository.DeleteByAssetId(assetId);
        Task removeRoleTask = userRepository.DeleteAssetRoles(assetId);

        await Task.WhenAll(new List<Task> { deleteAssetTask, deleteLocationsTask, removeRoleTask });

        await post.Send(new AssetDeletedEvent(assetId));
    }

    public async Task<AssetModel> Create(CreateAssetModel model)
    {
        UserDocument user = await userAccessor.Get();

        CreateAssetModelMapper.Map(model);
        await ValidateModel(model, user);

        AssetDocument assetDocument = await AddDocuments(model);

        DeviceType deviceType = deviceTypeRepository.GetById(model.DeviceTypeId);
        AssetModel asset = AssetModelMapper.Map(assetDocument, deviceType);

        await post.Send(new AssetCreatedEvent(asset));

        return asset;
    }

    public async Task<ListModel<AssetUserModel>> GetAssetUsers(string assetId)
    {
        AssetDocument asset = await assetRepository.GetById(assetId);
        List<UserDocument> users = await userRepository.GetUsersByIds(asset.UserRoles.Select(x => x.UserId));

        return AssetUserListModelMapper.Map(asset, users);
    }

    public async Task AddUserToAsset(string assetId, CreateAssetUserModel model)
    {
        AssetDocument asset = await assetRepository.GetById(assetId);
        asset.Return404IfNull();

        UserDocument? userDocument = await userRepository.GetByEmail(model.Email);

        if (userDocument == null)
        {
            throw new ValidationApiException().AddValidationError(nameof(model.Email),
                ValidationErrorCodes.NoUserWithEmail);
        }

        if (asset.UserRoles.Any(x => x.UserId == userDocument.Id))
        {
            throw new ValidationApiException().AddValidationError(nameof(model.Email),
                ValidationErrorCodes.UserAlreadyOnAsset);
        }

        if (!Enum.TryParse(model.Role, out AssetRoleType assetRoleType))
        {
            throw new ValidationApiException().AddValidationError(nameof(model.Role), ValidationErrorCodes.InvalidRole);
        }

        await assetRepository.AddUserToAsset(asset, userDocument, assetRoleType);
    }

    public async Task RemoveUserFromAsset(string assetId, string userId)
    {
        AssetDocument asset = await assetRepository.GetById(assetId);
        asset.Return404IfNull();

        AssetUserRoleElement assetUserRole = asset.UserRoles.FirstOrDefault(x => x.UserId == ObjectId.Parse(userId));

        asset.ThrowApiExceptionIfNull(HttpStatusCode.BadRequest);

        if (assetUserRole!.Role == AssetRoleType.Owner && asset.UserRoles.Count(x => x.Role == AssetRoleType.Owner) < 2)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "You cannot remove the only owner of the asset.");
        }

        await assetRepository.RemoveUserFromAsset(assetId, userId);
    }

    private async Task<AssetDocument> AddDocuments(CreateAssetModel model)
    {
        UserDocument currentUser = await userAccessor.Get();
        DeviceType deviceType = deviceTypeRepository.GetById(model.DeviceTypeId);

        AssetDocument assetDocument = AssetDocumentMapper.Map(model, currentUser);
        await repository.GetCollection<AssetDocument>().InsertOneAsync(assetDocument);

        DeviceDocument deviceDocument = DeviceDocumentMapper.Map(model, assetDocument.Id, currentUser.Id);
        await repository.GetCollection<DeviceDocument>().InsertOneAsync(deviceDocument);

        assetDocument.Device = AssetDeviceElementMapper.Map(deviceDocument, deviceType);

        await repository.GetCollection<UserDocument>().UpdateOneAsync(x => x.Id == currentUser.Id,
            Builders<UserDocument>.Update.AddToSet(x => x.AssetRoles, new UserAssetRoleElement
            {
                Id = ObjectId.GenerateNewId(),
                Role = AssetRoleType.Owner,
                AssetId = assetDocument.Id
            }));

        await assetRepository.SetActiveDevice(assetDocument.Id, deviceDocument.Id, deviceDocument.SerialNumber,
            deviceDocument.DeviceTypeId, deviceType.Protocol.Port);

        return assetDocument;
    }

    private async Task ValidateModel(CreateAssetModel model, UserDocument currentUser)
    {
        ValidationApiException validationException = new();

        if (await assetRepository.NameIsUsed(model.Name, currentUser.Id))
        {
            validationException.AddValidationError(nameof(model.Name), ValidationErrorCodes.AssetNameAlreadyUsed);
        }

        if (!deviceTypeRepository.Exists(model.DeviceTypeId))
        {
            validationException.AddValidationError(nameof(model.DeviceTypeId), ValidationErrorCodes.DeviceTypeInvalid);
        }

        if (await service.SerialNumberIsUsed(model.SerialNumber, model.DeviceTypeId))
        {
            validationException.AddValidationError(nameof(model.SerialNumber),
                ValidationErrorCodes.SerialNumberAlreadyUsed);
        }

        validationException.ThrowIfInvalid();
    }
}