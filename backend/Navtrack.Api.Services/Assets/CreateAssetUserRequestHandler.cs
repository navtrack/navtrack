using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Assets.Events;
using Navtrack.Api.Services.Assets.Mappers;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IRequestHandler<CreateAssetUserRequest>))]
public class CreateAssetUserRequestHandler(IAssetRepository assetRepository, IUserRepository userRepository)
    : BaseRequestHandler<CreateAssetUserRequest>
{
    private AssetDocument? asset;
    private UserDocument? user;

    public override async Task Validate(RequestValidationContext<CreateAssetUserRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();

        user = await userRepository.GetByEmail(context.Request.Model.Email);
        context.ValidationException.AddErrorIfNull(user, nameof(context.Request.Model.Email),
            ApiErrorCodes.User_000001_EmailNotFound);

        context.ValidationException.AddErrorIfTrue(
            user?.Assets?.Any(x => x.AssetId == asset.Id),
            nameof(context.Request.Model.Email), ApiErrorCodes.Asset_000001_UserAlreadyHasRole);
    }

    public override async Task Handle(CreateAssetUserRequest request)
    {
        UserAssetElement userAsset =
            UserAssetElementMapper.Map(asset!.Id, request.Model.UserRole, asset.OrganizationId);

        await userRepository.AddAssetToUser(user!.Id, userAsset);
    }

    public override IEvent GetEvent(CreateAssetUserRequest request)
    {
        return new AssetUserCreatedEvent(asset!.Id.ToString(), user!.Id.ToString());
    }
}