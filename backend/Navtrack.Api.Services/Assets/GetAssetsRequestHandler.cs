using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.Assets.Mappers;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Model.Organizations;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.DataAccess.Services.Organizations;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IRequestHandler<GetAssetsRequest, Model.Common.List<Asset>>))]
public class GetAssetsRequestHandler(
    INavtrackContextAccessor navtrackContextAccessor,
    IDeviceTypeRepository deviceTypeRepository,
    IAssetRepository assetRepository,
    IOrganizationRepository organizationRepository)
    : BaseRequestHandler<GetAssetsRequest, Model.Common.List<Asset>>
{
    private OrganizationDocument? organization;

    public override async Task Validate(RequestValidationContext<GetAssetsRequest> context)
    {
        organization = await organizationRepository.GetById(context.Request.OrganizationId);
        organization.Return404IfNull();
    }

    public override async Task<Model.Common.List<Asset>> Handle(GetAssetsRequest request)
    {
        System.Collections.Generic.List<AssetDocument> assets = await GetAssetsByOrganizationId(organization!.Id);

        System.Collections.Generic.List<string> assetDeviceTypes =
            assets.Where(x => x.Device != null)
                .Select(x => x.Device!.DeviceTypeId)
                .Distinct()
                .ToList();

        IEnumerable<DeviceType> deviceTypes =
            deviceTypeRepository.GetDeviceTypes().Where(x => assetDeviceTypes.Contains(x.Id));

        Model.Common.List<Asset> assetList = AssetListMapper.Map(assets, deviceTypes);

        return assetList;
    }
    
    
    private Task<System.Collections.Generic.List<AssetDocument>> GetAssetsByOrganizationId(ObjectId organizationId)
    {
        if (navtrackContextAccessor.NavtrackContext.HasOrganizationUserRole(organizationId.ToString(), OrganizationUserRole.Owner))
        {
            return assetRepository.GetByOrganizationId(organization!.Id);
        }

        List<ObjectId> assetIds =
            navtrackContextAccessor.NavtrackContext.User?.Assets?
                .Where(x => x.OrganizationId == organization!.Id)
                .Select(x => x.AssetId).ToList() ??
            [];

        List<ObjectId> teamIds =
            navtrackContextAccessor.NavtrackContext.User?.Teams?
                .Where(x => x.OrganizationId == organization!.Id)
                .Select(x => x.TeamId).ToList() ?? [];

        return assetRepository.GetAssetsByAssetAndTeamIds(assetIds, teamIds);
    }
}