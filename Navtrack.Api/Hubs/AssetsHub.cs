using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Assets.Requests;
using Navtrack.Api.Services.Extensions;

namespace Navtrack.Api.Hubs
{
    public class AssetsHub : BaseHub
    {
        public AssetsHub(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        
        public Task<IEnumerable<AssetResponseModel>> GetAll()
        {
            return HandleRequest<GetAllAssetsRequest, IEnumerable<AssetResponseModel>>(new GetAllAssetsRequest
            {
                UserId = Context.User.GetId()
            });
        }
    }
}