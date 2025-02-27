using System.Threading.Tasks;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Services.Common.Context;

public interface ICurrentContext
{
    Task<AssetDocument?> GetCurrentAsset();
}