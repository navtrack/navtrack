using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Model.Devices.Requests;
using Navtrack.Api.Services.RequestHandlers;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Devices
{
    [Service(typeof(IRequestHandler<GetDeviceStatisticsRequest, DeviceStatisticsResponseModel>))]
    public class GetDeviceStatisticsRequestHandler
        : BaseRequestHandler<GetDeviceStatisticsRequest, DeviceStatisticsResponseModel>
    {
        private readonly IRepository repository;

        public GetDeviceStatisticsRequestHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public override async Task<DeviceStatisticsResponseModel> Handle(GetDeviceStatisticsRequest request)
        {
            DeviceEntity entity = await repository.GetEntities<DeviceEntity>()
                .FirstOrDefaultAsync(x => x.Id == request.DeviceId &&
                                          x.Asset.Users.Any(y =>
                                              y.UserId == request.UserId && y.RoleId == (int) UserAssetRole.Owner));

            if (entity != null)
            {
                DeviceStatisticsResponseModel model = new DeviceStatisticsResponseModel
                {
                    Locations = await repository.GetEntities<LocationEntity>()
                        .CountAsync(x => x.DeviceId == entity.Id),
                    FirstLocationDateTime = (await repository.GetEntities<LocationEntity>()
                            .Where(x => x.DeviceId == request.DeviceId)
                            .OrderBy(x => x.DateTime)
                            .FirstOrDefaultAsync())
                        ?.DateTime,
                    LastLocationDateTime = (await repository.GetEntities<LocationEntity>()
                            .Where(x => x.DeviceId == request.DeviceId)
                            .OrderByDescending(x => x.DateTime)
                            .FirstOrDefaultAsync())
                        ?.DateTime,
                    Connections = await repository.GetEntities<DeviceConnectionEntity>()
                        .CountAsync(x => x.DeviceId == request.DeviceId)
                };

                return model;
            }

            return null;
        }
    }
}