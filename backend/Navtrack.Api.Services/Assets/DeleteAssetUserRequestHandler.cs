using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Services.Assets.Events;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IRequestHandler<DeleteAssetUserRequest>))]
public class DeleteAssetUserRequestHandler(
    IAssetRepository assetRepository,
    IUserRepository userRepository) : BaseRequestHandler<DeleteAssetUserRequest>
{
    private AssetEntity? asset;
    private UserEntity? user;

    public override async Task Validate(RequestValidationContext<DeleteAssetUserRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();

        user = await userRepository.GetById(context.Request.UserId);
        user.Return404IfNull();

        AssetEntity? assetUserRole = user.Assets.FirstOrDefault(x => x.Id == asset.Id);
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