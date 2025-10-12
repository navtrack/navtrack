using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.Assets.Mappers;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Devices;
using Navtrack.Database.Services.Organizations;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IRequestHandler<GetAssetsRequest, Model.Common.ListModel<AssetModel>>))]
public class GetAssetsRequestHandler(
    INavtrackContextAccessor navtrackContextAccessor,
    IDeviceTypeRepository deviceTypeRepository,
    IAssetRepository assetRepository,
    IOrganizationRepository organizationRepository)
    : BaseRequestHandler<GetAssetsRequest, Model.Common.ListModel<AssetModel>>
{
    private OrganizationEntity? organization;

    public override async Task Validate(RequestValidationContext<GetAssetsRequest> context)
    {
        organization = await organizationRepository.GetById(context.Request.OrganizationId);
        organization.Return404IfNull();
    }

    public override async Task<Model.Common.ListModel<AssetModel>> Handle(GetAssetsRequest request)
    {
        List<AssetEntity> assets = await GetAssetsByOrganizationId(organization!.Id);

        List<string> assetDeviceTypes =
            assets.Where(x => x.Device != null)
                .Select(x => x.Device!.DeviceTypeId)
                .Distinct()
                .ToList();

        IEnumerable<DeviceType> deviceTypes =
            deviceTypeRepository.GetDeviceTypes().Where(x => assetDeviceTypes.Contains(x.Id));

        Model.Common.ListModel<AssetModel> assetList = AssetListMapper.Map(assets, deviceTypes);

        return assetList;
    }
    
    private Task<List<AssetEntity>> GetAssetsByOrganizationId(Guid organizationId)
    {
        if (navtrackContextAccessor.NavtrackContext.HasOrganizationUserRole(organizationId, OrganizationUserRole.Owner))
        {
            return assetRepository.GetByOrganizationId(organization!.Id);
        }

        List<Guid> assetIds =
            navtrackContextAccessor.NavtrackContext.User?.Assets
                .Where(x => x.OrganizationId == organization!.Id)
                .Select(x => x.Id).ToList() ??
            [];

        List<Guid> teamIds =
            navtrackContextAccessor.NavtrackContext.User?.Teams?
                .Where(x => x.OrganizationId == organization!.Id)
                .Select(x => x.Id).ToList() ?? [];

        return assetRepository.GetByAssetAndTeamIds(assetIds, teamIds);
    }
}