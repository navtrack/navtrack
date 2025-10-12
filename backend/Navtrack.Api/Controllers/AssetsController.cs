using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Controllers.Shared;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Assets;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Organizations;

namespace Navtrack.Api.Controllers;

public class AssetsController(IRequestHandler requestHandler) : BaseAssetsController(requestHandler)
{
    [HttpGet(ApiPaths.OrganizationAssets)]
    [ProducesResponseType(typeof(ListModel<AssetModel>), StatusCodes.Status200OK)]
    [AuthorizeOrganization(OrganizationUserRole.Member)]
    public async Task<ListModel<AssetModel>> GetList([FromRoute] string organizationId)
    {
        ListModel<AssetModel> result =
            await requestHandler.Handle<GetAssetsRequest, ListModel<AssetModel>>(
                new GetAssetsRequest
                {
                    OrganizationId = organizationId
                });

        return result;
    }
}