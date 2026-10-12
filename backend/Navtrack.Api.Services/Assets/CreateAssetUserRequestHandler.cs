using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Assets.Mappers;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.RequestContext;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IRequestHandler<CreateAssetUserRequest>))]
public class CreateAssetUserRequestHandler(IAssetRepository assetRepository, IUserRepository userRepository, INavtrackRequestContextAccessor navtrackRequestContextAccessor)
    : BaseRequestHandler<CreateAssetUserRequest>
{
    public override async Task Handle(CreateAssetUserRequest request)
    {
        AssetEntity? asset = await assetRepository.GetById(request.AssetId);
        asset.Return404IfNull();

        UserEntity? user = await userRepository.GetByEmail(request.Model.Email);
        ValidationApiException validationException = new();

        validationException.AddErrorIfNull(user, nameof(request.Model.Email), ApiErrorCodes.User_EmailNotFound);
        validationException.AddErrorIfTrue(
            user?.Assets.Any(x => x.Id == asset.Id),
            nameof(request.Model.Email), ApiErrorCodes.Asset_UserAlreadyHasRole);
        validationException.ThrowIfInvalid();

        AssetUserEntity userAsset = UserAssetElementMapper.Map(user.Id, asset!.Id, request.Model.UserRole, navtrackRequestContextAccessor.NavtrackContext.CurrentUser.Id);

        await userRepository.AddAssetToUser(user.Id, userAsset);
    }
}
