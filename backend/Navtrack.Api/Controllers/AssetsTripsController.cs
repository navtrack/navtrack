using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Controllers.Shared;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Trips;
using Navtrack.DataAccess.Model.Assets;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.AssetsTrips)]
public class AssetsTripsController(IRequestHandler requestHandler) : ControllerBase
{
    [HttpGet(ApiPaths.AssetTrips)]
    [ProducesResponseType(typeof(TripList), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetUserRole.Viewer)]
    public async Task<TripList> GetList([FromRoute] string assetId, [FromQuery] TripFilter filter)
    {
        TripList result = await requestHandler.Handle<GetAssetTripsRequest, TripList>(
            new GetAssetTripsRequest
            {
                AssetId = assetId,
                Filter = filter
            });

        return result;
    }
}