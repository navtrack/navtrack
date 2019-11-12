using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Web.Model.Models;

namespace Navtrack.Web.Services
{
    public interface IAssetService
    {
        Task<List<AssetModel>> Get();
    }
}