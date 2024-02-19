using System.Threading.Tasks;
using Navtrack.Api.Model.Positions;
using Navtrack.Api.Services.Mappers.Positions;
using Navtrack.DataAccess.Model.Positions;
using Navtrack.DataAccess.Services.Positions;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Positions;

[Service(typeof(IPositionService))]
public class PositionService(IPositionRepository positionRepository) : IPositionService
{
    public async Task<PositionListModel> GetPositions(string assetId, PositionFilterModel positionFilter, int page, int size)
    {
        GetPositionsResult positions = await positionRepository.GetPositions(assetId, positionFilter, page, size);

        PositionListModel model = PositionListMapper.Map(positions);
    
        return model;
    }
}