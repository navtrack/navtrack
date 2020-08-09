using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Assets.Requests;
using Navtrack.Api.Services.Extensions;

namespace Navtrack.Api.Controllers
{
    [Authorize]
    public class AssetsController : BaseController
    {
        public AssetsController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpPost]
        public Task<ActionResult<AddAssetResponseModel>> Add([FromBody]AddAssetRequestModel model)
        {
            return HandleRequest<AddAssetRequest, AddAssetResponseModel>(new AddAssetRequest
            {
                Body = model,
                UserId = User.GetId()
            });
        }

        [HttpGet]
        public Task<ActionResult<IEnumerable<AssetResponseModel>>> GetAll()
        {
            return HandleRequest<GetAllAssetsRequest, IEnumerable<AssetResponseModel>>(new GetAllAssetsRequest
            {
                UserId = User.GetId()
            });
        }

        [HttpGet("{id}")]
        public Task<ActionResult<AssetResponseModel>> Get([FromRoute] int id)
        {
            return HandleRequest<GetAssetByIdRequest, AssetResponseModel>(new GetAssetByIdRequest
            {
                AssetId = id,
                UserId = User.GetId()
            });
        }

        [HttpPost("{assetId}/settings/rename")]
        public Task<IActionResult> Rename([FromRoute] int assetId, [FromBody] RenameAssetRequestModel model)
        {
            return HandleRequest(new RenameAssetRequest
            {
                AssetId = assetId,
                Body = model,
                UserId = User.GetId()
            });
        }

        [HttpPut("{assetId}/device")]
        public Task<IActionResult> ChangeDevice([FromRoute] int assetId, [FromBody] ChangeDeviceRequestModel model)
        {
            return HandleRequest(new ChangeDeviceRequest
            {
                Body = model,
                UserId = User.GetId(),
                AssetId = assetId
            });
        }
    }
}