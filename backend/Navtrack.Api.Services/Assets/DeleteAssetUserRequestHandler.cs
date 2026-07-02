using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IRequestHandler<DeleteAssetUserRequest>))]
public class DeleteAssetUserRequestHandler(
    IAssetRepository assetRepository,
    IUserRepository userRepository) : BaseRequestHandler<DeleteAssetUserRequest>
{
    public override async Task Handle(DeleteAssetUserRequest request)
    {
        AssetEntity? asset = await assetRepository.GetById(request.AssetId);
        asset.Return404IfNull();

        UserEntity? user = await userRepository.GetById(request.UserId);
        user.Return404IfNull();

        AssetEntity? assetUserRole = user.Assets.FirstOrDefault(x => x.Id == asset.Id);
        assetUserRole.Return404IfNull();

        await userRepository.RemoveAssetFromUser(asset.Id, user.Id);
    }
}
