using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Positions;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Positions;
using Navtrack.Listener.Mappers;
using Navtrack.Listener.Models;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Services;

[Service(typeof(IPositionService))]
public class PositionService(IPositionRepository positionRepository, IAssetRepository assetRepository)
    : IPositionService
{
    public async Task Save(ObjectId connectionId, Device device, IEnumerable<Position> positions)
    {
        if (device is { AssetId: not null, DeviceId: not null })
        {
            List<PositionDocument> mappedPositions = positions.Select(x => PositionDocumentMapper.Map(x, device, connectionId))
                .OrderBy(x => x.Date)
                .ToList();

            await positionRepository.AddRange(mappedPositions);

            PositionDocument? latestPosition = mappedPositions.MaxBy(x => x.Date);

            if (latestPosition != null && (device.MaxDate == null || device.MaxDate < latestPosition.Date))
            {
                await assetRepository.SetPosition(device.AssetId.Value, latestPosition);
                device.MaxDate = latestPosition.Date;
            }
        }
    }
}