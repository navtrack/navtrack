using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Controllers.Shared;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Assets;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Organizations;

namespace Navtrack.Api.Controllers;

public class AssetsController(IRequestHandler requestHandler) : BaseAssetsController(requestHandler)
{
    [HttpGet(ApiPaths.OrganizationAssets)]
    [ProducesResponseType(typeof(List<Asset>), StatusCodes.Status200OK)]
    [AuthorizeOrganization(OrganizationUserRole.Member)]
    public async Task<List<Asset>> GetList([FromRoute] string organizationId)
    {
        List<Asset> result =
            await requestHandler.Handle<GetAssetsRequest, List<Asset>>(
                new GetAssetsRequest
                {
                    OrganizationId = organizationId
                });

        return result;
    }
}