using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Controllers.Shared;
using Navtrack.Api.Model.Geocode;
using Navtrack.Api.Services.Geocoding;
using Navtrack.Api.Services.Requests;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public class GeocodeController(IRequestHandler requestHandler) : NavtrackControllerBase(requestHandler)
{
    [HttpGet(ApiPaths.GeocodeReverse)]
    [ProducesResponseType(typeof(LocationModel), StatusCodes.Status200OK)]
    public Task<LocationModel> Reverse([FromQuery] double lat, [FromQuery] double lon) =>
        Query<GetReverseGeocodeRequest, LocationModel>(new GetReverseGeocodeRequest
        {
            Latitude = lat,
            Longitude = lon
        });
}
