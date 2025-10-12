using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Geocode;
using Navtrack.Api.Services.Geocoding;
using Navtrack.Api.Services.Requests;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public class GeocodeController(IRequestHandler requestHandler)
{
    [HttpGet(ApiPaths.GeocodeReverse)]
    [ProducesResponseType(typeof(LocationModel), StatusCodes.Status200OK)]
    public async Task<LocationModel> Reverse([FromQuery]double lat, [FromQuery]double lon)
    {
        LocationModel result = await requestHandler.Handle<GetReverseGeocodeRequest, LocationModel>(
            new GetReverseGeocodeRequest
            {
                Latitude = lat,
                Longitude = lon
            });

        return result;
    }
}