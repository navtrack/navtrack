using System;
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
    public async Task<SavePositionsResult> Save(Device device, DateTime maxEndDate,
        ObjectId connectionId, IEnumerable<Location> locations)
    {
        SavePositionsResult result = new();

        if (device is { AssetId: not null, DeviceId: not null })
        {
            List<PositionElement> mapped = locations.Select(PositionElementMapper.Map)
                .OrderBy(x => x.Date)
                .ToList();

            PositionElement startPosition = mapped.First();
            PositionElement endPosition = mapped.Last();

            if (device.PositionGroupId == null)
            {
                PositionGroupDocument positionGroupDocument = new()
                {
                    DeviceId = device.DeviceId.Value,
                    AssetId = device.AssetId.Value,
                    CreatedDate = DateTime.UtcNow,
                    Positions = mapped,
                    ConnectionId = connectionId,
                    StartDate = startPosition.Date,
                    EndDate = endPosition.Date
                };

                await positionRepository.Add(positionGroupDocument);

                result.PositionGroupId = positionGroupDocument.Id;
            }
            else
            {
                await positionRepository.AddPositions(device.PositionGroupId.Value, maxEndDate, mapped);
            }

            bool shouldUpdateAsset = await assetRepository.IsNewerPositionDate(device.AssetId.Value, endPosition.Date);

            if (shouldUpdateAsset)
            {
                await assetRepository.SetPosition(device.AssetId.Value, endPosition);
            }

            result.Success = true;
            result.MaxDate = endPosition.Date;
        }

        return result;
    }
}