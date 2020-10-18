using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.CommandHandler;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Devices
{
    [Service(typeof(ICommandHandler<GetDeviceByIdCommand, DeviceResponseModel>))]
    public class GetDeviceByIdCommandHandler : BaseCommandHandler<GetDeviceByIdCommand, DeviceResponseModel>
    {
        private readonly IMapper mapper;
        private readonly IRepository repository;

        public GetDeviceByIdCommandHandler(IMapper mapper, IRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public override async Task<DeviceResponseModel> Handle(GetDeviceByIdCommand command)
        {
            DeviceEntity entity = await repository.GetEntities<DeviceEntity>()
                .FirstOrDefaultAsync(x => x.Id == command.DeviceId &&
                                          x.Asset.Users.Any(y =>
                                              y.UserId == command.UserId && y.RoleId == (int) UserAssetRole.Owner));

            if (entity != null)
            {
                DeviceResponseModel model = mapper.Map<DeviceEntity, DeviceResponseModel>(entity);

                model.LocationsCount = await repository.GetEntities<LocationEntity>()
                    .CountAsync(x => x.DeviceId == entity.Id);

                return model;
            }

            return null;
        }
    }
}