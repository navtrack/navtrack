using System.Threading.Tasks;
using Navtrack.DataAccess.Model;
using Navtrack.Web.Models;
using Navtrack.Web.Services.Generic;

namespace Navtrack.Web.Services
{
    public interface IAssetService : IGenericService<Asset, AssetModel>
    {
        Task<AssetStatsModel> GetStats();
    }
}