using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.IdentityServer;

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
        public Task<ActionResult<AddAssetResponseModel>> Add([FromBody] AddAssetRequestModel model)
        {
            return HandleCommand<AddAssetCommand, AddAssetResponseModel>(new AddAssetCommand
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
        public Task<ActionResult<GetTripsResponseModel>> GetTrips([FromRoute] int assetId, 
            [FromQuery] GetTripsRequestModel model)
        {
            return HandleCommand<GetTripsCommand, GetTripsResponseModel>(
                new GetTripsCommand
                {
                    AssetId = assetId,
                    UserId = User.GetId(),
                    Model = model
                });
        }
        
        
        [HttpGet("{assetId}/locations")]
        public Task<ActionResult<GetLocationsResponseModel>> GetLocations([FromRoute] int assetId, 
            [FromQuery] GetLocationsRequestModel model)
        {
            return HandleCommand<GetLocationsCommand, GetLocationsResponseModel>(
                new GetLocationsCommand
                {
                    AssetId = assetId,
                    Model = model,
                    UserId = User.GetId()
                });
        }

        [HttpGet("{assetId}/locations/latest")]
        public Task<ActionResult<LocationResponseModel>> GetLatestLocation(int assetId)
        {
            return HandleCommand<GetLatestLocationCommand, LocationResponseModel>(
                new GetLatestLocationCommand
                {
                    AssetId = assetId,
                    UserId = User.GetId()
                });
        }
    }
}