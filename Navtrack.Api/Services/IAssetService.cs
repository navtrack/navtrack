using Navtrack.Api.Models;
using Navtrack.Api.Services.Generic;
using Navtrack.DataAccess.Model;

namespace Navtrack.Api.Services
{
    public interface IAssetService : IGenericService<Asset, AssetModel>
    {
    }
}