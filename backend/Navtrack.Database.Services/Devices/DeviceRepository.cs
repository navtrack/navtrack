using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Postgres;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Database.Services.Devices;

[Service(typeof(IDeviceRepository))]
public class DeviceRepository(IPostgresRepository repository)
    : GenericPostgresRepository<DeviceEntity>(repository), IDeviceRepository
{
    public override async Task Add(DeviceEntity document)
    {
        await base.Add(document);
        await UpdateDeviceCount(document.OrganizationId);
    }

    private async Task UpdateDeviceCount(Guid organizationId)
    {
        int devices = await repository.GetQueryable<DeviceEntity>()
            .CountAsync(x => x.OrganizationId == organizationId);

        await repository.GetQueryable<OrganizationEntity>()
            .Where(x => x.Id == organizationId)
            .ExecuteUpdateAsync(x => x.SetProperty(o => o.DevicesCount, devices));
    }

    public Task<bool> SerialNumberIsUsed(string serialNumber, int protocolPort, Guid? excludeAssetId = null)
    {
        return repository.GetQueryable<AssetEntity>()
            .AnyAsync(x => x.Device != null && x.Device.SerialNumber == serialNumber &&
                           x.Device.ProtocolPort == protocolPort &&
                           (excludeAssetId == null || x.Id != excludeAssetId));
    }

    public Task<List<DeviceEntity>> GetDevicesByAssetId(Guid assetId)
    {
        return repository.GetQueryable<DeviceEntity>()
            .Where(x => x.AssetId == assetId).ToListAsync();
    }

    public Task<bool> IsActive(Guid assetId, Guid deviceId)
    {
        return repository.GetQueryable<AssetEntity>()
            .AnyAsync(x => x.Id == assetId && x.DeviceId == deviceId);
    }

    public Task DeleteByAssetId(Guid id)
    {
        return repository.GetQueryable<DeviceEntity>().Where(x => x.Id == id).ExecuteDeleteAsync<DeviceEntity>();
    }
}