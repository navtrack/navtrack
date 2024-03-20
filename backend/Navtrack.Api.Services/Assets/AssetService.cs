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
    ICurrentUserAccessor currentUserAccessor,
    IMessageRepository messageRepository,
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
        UserDocument currentUser = await currentUserAccessor.Get();

        return AssetModelMapper.Map(currentUser, asset, deviceType);
    }

    public async Task<ListModel<AssetModel>> GetAssets()
    {
        UserDocument currentUser = await currentUserAccessor.Get();
        List<ObjectId> assetIds = currentUser.AssetRoles?.Select(x => x.AssetId).ToList() ??
                                  Enumerable.Empty<ObjectId>().ToList();
        List<AssetDocument> assets = await assetRepository.GetAssetsByIds(assetIds);

        List<string> assetDeviceTypes =
            assets.Select(x => x.Device.DeviceTypeId).Distinct().ToList();

        IEnumerable<DeviceType> deviceTypes =
            deviceTypeRepository.GetDeviceTypes().Where(x => assetDeviceTypes.Contains(x.Id));

        ListModel<AssetModel> model = AssetListMapper.Map(currentUser, assets, deviceTypes);

        return model;
    }

    public async Task Update(string assetId, UpdateAssetModel model)
    {
        AssetModel asset = await GetById(assetId);
        asset.Return404IfNull();

        if (!string.IsNullOrEmpty(model.Name) && asset.Name != model.Name)
        {
            UserDocument user = await currentUserAccessor.Get();

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
        Task deleteLocationsTask = messageRepository.DeleteByAssetId(assetId);
        Task removeRoleTask = userRepository.DeleteAssetRoles(assetId);

        await Task.WhenAll(new List<Task> { deleteAssetTask, deleteLocationsTask, removeRoleTask });

        await post.Send(new AssetDeletedEvent(assetId));
    }

    public async Task<AssetModel> Create(CreateAssetModel model)
    {
        UserDocument currentUser = await currentUserAccessor.Get();

        CreateAssetModelMapper.Map(model);
        await ValidateModel(model, currentUser);

        AssetDocument assetDocument = await AddDocuments(model);

        DeviceType deviceType = deviceTypeRepository.GetById(model.DeviceTypeId);
        AssetModel asset = AssetModelMapper.Map(currentUser, assetDocument, deviceType);

        await post.Send(new AssetCreatedEvent(asset.Id));

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

        if (!Enum.TryParse(model.Role, out AssetRoleType assetRoleType) || assetRoleType == AssetRoleType.Owner)
        {
            throw new ValidationApiException().AddValidationError(nameof(model.Role), ValidationErrorCodes.InvalidRole);
        }
        
        AssetUserRoleElement userRole = AssetUserRoleElementMapper.Map(userDocument.Id, assetRoleType);
        UserAssetRoleElement assetRole = UserAssetRoleElementMapper.Map(asset.Id, assetRoleType);

        await assetRepository.AddUserToAsset(userRole, assetRole);

        await post.Send(new AssetUserAddedEvent(assetId, userDocument.Id.ToString()));
    }

    public async Task RemoveUserFromAsset(string assetId, string userId)
    {
        AssetDocument asset = await assetRepository.GetById(assetId);
        asset.Return404IfNull();

        AssetUserRoleElement? assetUserRole = asset.UserRoles.FirstOrDefault(x => x.UserId == ObjectId.Parse(userId));

        asset.ThrowApiExceptionIfNull(HttpStatusCode.BadRequest);

        if (assetUserRole!.Role == AssetRoleType.Owner && asset.UserRoles.Count(x => x.Role == AssetRoleType.Owner) < 2)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "You cannot remove the only owner of the asset.");
        }

        await assetRepository.RemoveUserFromAsset(assetId, userId);

        await post.Send(new AssetUserDeletedEvent(assetId, userId));
    }

    private async Task<AssetDocument> AddDocuments(CreateAssetModel model)
    {
        UserDocument currentUser = await currentUserAccessor.Get();
        DeviceType deviceType = deviceTypeRepository.GetById(model.DeviceTypeId);

        AssetDocument assetDocument = AssetDocumentMapper.Map(model, currentUser);
        await repository.GetCollection<AssetDocument>().InsertOneAsync(assetDocument);

        DeviceDocument deviceDocument = DeviceDocumentMapper.Map(assetDocument.Id, model, currentUser.Id);
        await repository.GetCollection<DeviceDocument>().InsertOneAsync(deviceDocument);

        assetDocument.Device = AssetDeviceElementMapper.Map(deviceDocument, deviceType);

        await repository.GetCollection<UserDocument>().UpdateOneAsync(x => x.Id == currentUser.Id,
            Builders<UserDocument>.Update.AddToSet(x => x.AssetRoles, new UserAssetRoleElement
            {
                Role = AssetRoleType.Owner,
                AssetId = assetDocument.Id,
                CreatedDate = DateTime.UtcNow
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