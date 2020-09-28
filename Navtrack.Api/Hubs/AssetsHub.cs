using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.IdentityServer;

namespace Navtrack.Api.Hubs
{
    public class AssetsHub : BaseHub
    {
        public AssetsHub(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        
        public Task<IEnumerable<AssetModel>> GetAll()
        {
            return HandleRequest<GetAllAssetsCommand, IEnumerable<AssetModel>>(new GetAllAssetsCommand
            {
                UserId = Context.User.GetId()
            });
        }
    }
}