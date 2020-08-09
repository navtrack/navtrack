using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Locations;
using Navtrack.Api.Model.Locations.Requests;
using Navtrack.Api.Services.Extensions;

namespace Navtrack.Api.Controllers
{
    public class LocationsController : BaseController
    {
        public LocationsController(IServiceProvider serviceProvider) : base(
            serviceProvider)
        {
        }

        [HttpGet("history")]
        public Task<ActionResult<IEnumerable<LocationResponseModel>>> GetHistory(
            [FromQuery] LocationHistoryRequestModel model)
        {
            return HandleRequest<GetLocationsHistoryRequest, IEnumerable<LocationResponseModel>>(
                new GetLocationsHistoryRequest
                {
                    Body = model,
                    UserId = User.GetId()
                });
        }

        [HttpGet("{assetId}/latest")]
        public Task<ActionResult<LocationResponseModel>> GetLatest(int assetId)
        {
            return HandleRequest<GetLatestLocationRequest, LocationResponseModel>(
                new GetLatestLocationRequest
                {
                    AssetId = assetId,
                    UserId = User.GetId()
                });
        }
    }
}