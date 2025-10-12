using System.Threading.Tasks;
using Navtrack.Database.Model.Assets;

namespace Navtrack.Api.Services.Common.Context;

public interface ICurrentContext
{
    Task<AssetEntity?> GetCurrentAsset();
}