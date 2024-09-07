using System.Threading.Tasks;
using Navtrack.Api.Model.Messages;

namespace Navtrack.Api.Services.Positions;

public interface IPositionService
{
    Task<MessageListModel> GetPositions(string assetId, MessageFilterModel messageFilter, int page, int size);
}