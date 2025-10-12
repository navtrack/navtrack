using System.Threading.Tasks;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Assets.Mappers;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Services.Assets;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IRequestHandler<GetAssetUsersRequest, ListModel<AssetUserModel>>))]
public class GetAssetUsersRequestHandler(IAssetRepository assetRepository) : BaseRequestHandler<GetAssetUsersRequest, ListModel<AssetUserModel>>
{
    private AssetEntity? asset;

    public override async Task Validate(RequestValidationContext<GetAssetUsersRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();
    }

    public override async Task<ListModel<AssetUserModel>> Handle(GetAssetUsersRequest request)
    {
        System.Collections.Generic.List<AssetUserEntity> assetUsers = await assetRepository.GetUsers(asset!.Id);

        ListModel<AssetUserModel> result = ListMapper.Map(assetUsers, AssetUserMapper.Map);

        return result;
    }
}