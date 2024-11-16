using System.Threading.Tasks;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Assets.Mappers;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IRequestHandler<GetAssetUsersRequest, List<AssetUser>>))]
public class GetAssetUsersRequestHandler(
    IAssetRepository assetRepository,
    IUserRepository userRepository) : BaseRequestHandler<GetAssetUsersRequest, List<AssetUser>>
{
    private AssetDocument? asset;

    public override async Task Validate(RequestValidationContext<GetAssetUsersRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();
    }

    public override async Task<List<AssetUser>> Handle(GetAssetUsersRequest request)
    {
        System.Collections.Generic.List<UserDocument> users = await userRepository.GetByAssetId(asset!.Id);

        List<AssetUser> result = ListMapper.Map(users, x => AssetUserMapper.Map(x, asset));

        return result;
    }
}