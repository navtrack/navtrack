using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Api.Model.Locations;
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
        List<PositionElement> positions = await positionRepository.GetPositions(assetId, positionFilter);

        PositionListModel model = PositionListMapper.Map(positions);
    
        return model;
    }
}