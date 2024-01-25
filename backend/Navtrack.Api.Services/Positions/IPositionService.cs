using System.Threading.Tasks;
using Navtrack.Api.Model.Positions;

namespace Navtrack.Api.Services.Positions;

public interface IPositionService
{
    Task<PositionListModel> GetPositions(string assetId, PositionFilterModel positionFilter, int page, int size);
}