using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.CommandHandler;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Devices
{
    [Service(typeof(ICommandHandler<GetDeviceStatisticsCommand, DeviceStatisticsResponseModel>))]
    public class GetDeviceStatisticsCommandHandler
        : BaseCommandHandler<GetDeviceStatisticsCommand, DeviceStatisticsResponseModel>
    {
        private readonly IRepository repository;

        public GetDeviceStatisticsCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public override async Task<DeviceStatisticsResponseModel> Handle(GetDeviceStatisticsCommand command)
        {
            DeviceEntity entity = await repository.GetEntities<DeviceEntity>()
                .FirstOrDefaultAsync(x => x.Id == command.DeviceId &&
                                          x.Asset.Users.Any(y =>
                                              y.UserId == command.UserId && y.RoleId == (int) UserAssetRole.Owner));

            if (entity != null)
            {
                DeviceStatisticsResponseModel model = new DeviceStatisticsResponseModel
                {
                    Locations = await repository.GetEntities<LocationEntity>()
                        .CountAsync(x => x.DeviceId == entity.Id),
                    FirstLocationDateTime = (await repository.GetEntities<LocationEntity>()
                            .Where(x => x.DeviceId == command.DeviceId)
                            .OrderBy(x => x.DateTime)
                            .FirstOrDefaultAsync())
                        ?.DateTime,
                    LastLocationDateTime = (await repository.GetEntities<LocationEntity>()
                            .Where(x => x.DeviceId == command.DeviceId)
                            .OrderByDescending(x => x.DateTime)
                            .FirstOrDefaultAsync())
                        ?.DateTime,
                    Connections = await repository.GetEntities<DeviceConnectionEntity>()
                        .CountAsync(x => x.DeviceId == command.DeviceId)
                };

                return model;
            }

            return null;
        }
    }
}