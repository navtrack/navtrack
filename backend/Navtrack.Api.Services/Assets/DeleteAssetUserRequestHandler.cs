using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Services.Assets.Events;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IRequestHandler<DeleteAssetUserRequest>))]
public class DeleteAssetUserRequestHandler(
    IAssetRepository assetRepository,
    IUserRepository userRepository) : BaseRequestHandler<DeleteAssetUserRequest>
{
    private AssetDocument? asset;
    private UserDocument? user;

    public override async Task Validate(RequestValidationContext<DeleteAssetUserRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();

        user = await userRepository.GetById(context.Request.UserId);
        user.Return404IfNull();

        UserAssetElement? assetUserRole = user.Assets?.FirstOrDefault(x => x.AssetId == asset.Id);
        assetUserRole.Return404IfNull();
    }

    public override Task Handle(DeleteAssetUserRequest request)
    {
        return userRepository.RemoveAssetFromUser(asset!.Id, user!.Id);
    }

    public override IEvent GetEvent(DeleteAssetUserRequest request)
    {
        return new AssetUserDeletedEvent(asset!.Id.ToString(), user!.Id.ToString());
    }
}