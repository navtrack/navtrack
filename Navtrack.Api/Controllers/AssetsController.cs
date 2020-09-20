using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.Extensions;

namespace Navtrack.Api.Controllers
{
    [Authorize]
    public class AssetsController : BaseController
    {
        public AssetsController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpGet("{id}")]
        public Task<ActionResult<AssetModel>> Get([FromRoute] int id)
        {
            return HandleCommand<GetAssetByIdCommand, AssetModel>(new GetAssetByIdCommand
            {
                AssetId = id,
                UserId = User.GetId()
            });
        }

        [HttpGet]
        public Task<ActionResult<IEnumerable<AssetModel>>> GetAll()
        {
            return HandleCommand<GetAllAssetsCommand, IEnumerable<AssetModel>>(new GetAllAssetsCommand
            {
                UserId = User.GetId()
            });
        }

        [HttpPost]
        public Task<ActionResult<AddAssetModel>> Add([FromBody] AddAssetRequestModel model)
        {
            return HandleCommand<AddAssetCommand, AddAssetModel>(new AddAssetCommand
            {
                Model = model,
                UserId = User.GetId()
            });
        }

        [HttpDelete("{assetId}")]
        public Task<IActionResult> Delete([FromRoute] int assetId)
        {
            return HandleCommand(new DeleteAssetCommand
            {
                UserId = User.GetId(),
                AssetId = assetId
            });
        }

        [HttpPost("{assetId}/settings/rename")]
        public Task<IActionResult> Rename([FromRoute] int assetId, [FromBody] RenameAssetRequestModel model)
        {
            return HandleCommand(new RenameAssetCommand
            {
                AssetId = assetId,
                Model = model,
                UserId = User.GetId()
            });
        }

        [HttpPut("{assetId}/device")]
        public Task<IActionResult> ChangeDevice([FromRoute] int assetId, [FromBody] ChangeDeviceRequestModel model)
        {
            return HandleCommand(new ChangeDeviceCommand
            {
                Model = model,
                UserId = User.GetId(),
                AssetId = assetId
            });
        }

        [HttpGet("{assetId}/trips")]
        public Task<ActionResult<GetTripsModel>> GetTrips([FromRoute] int assetId)
        {
            return HandleCommand<GetTripsCommand, GetTripsModel>(
                new GetTripsCommand
                {
                    AssetId = assetId,
                    UserId = User.GetId()
                });
        }
        
        
        [HttpGet("{assetId}/locations")]
        public Task<ActionResult<IEnumerable<LocationModel>>> GetLocations([FromRoute] int assetId, 
            [FromQuery] LocationHistoryRequestModel model)
        {
            return HandleCommand<GetLocationsCommand, IEnumerable<LocationModel>>(
                new GetLocationsCommand
                {
                    AssetId = assetId,
                    Model = model,
                    UserId = User.GetId()
                });
        }

        [HttpGet("{assetId}/locations/latest")]
        public Task<ActionResult<LocationModel>> GetLatestLocation(int assetId)
        {
            return HandleCommand<GetLatestLocationCommand, LocationModel>(
                new GetLatestLocationCommand
                {
                    AssetId = assetId,
                    UserId = User.GetId()
                });
        }
    }
}