using System.Threading.Tasks;
using Navtrack.Api.Model.Positions;
using Navtrack.Api.Services.Mappers.Positions;
using Navtrack.DataAccess.Model.Positions;
using Navtrack.DataAccess.Services.Positions;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Positions;

[Service(typeof(IPositionService))]
public class PositionService(IMessageRepository messageRepository) : IPositionService
{
    public async Task<PositionListModel> GetPositions(string assetId, PositionFilterModel positionFilter, int page,
        int size)
    {
        GetMessagesResult messages = await messageRepository.GetMessages(new GetMessagesOptions
        {
            AssetId = assetId,
            PositionFilter = positionFilter,
            Page = page,
            Size = size
        });

        PositionListModel model = PositionListMapper.Map(messages);

        return model;
    }
}