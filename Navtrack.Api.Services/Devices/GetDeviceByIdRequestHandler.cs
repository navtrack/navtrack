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
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Devices
{
    [Service(typeof(IRequestHandler<GetDeviceByIdRequest, DeviceModel>))]
    public class GetDeviceByIdRequestHandler : BaseRequestHandler<GetDeviceByIdRequest, DeviceModel>
    {
        private readonly IMapper mapper;
        private readonly IRepository repository;

        public GetDeviceByIdRequestHandler(IMapper mapper, IRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public override async Task<DeviceModel> Handle(GetDeviceByIdRequest request)
        {
            DeviceEntity entity = await repository.GetEntities<DeviceEntity>()
                .FirstOrDefaultAsync(x => x.Id == request.DeviceId &&
                                          x.Asset.Users.Any(y =>
                                              y.UserId == request.UserId && y.RoleId == (int) UserAssetRole.Owner));

            if (entity != null)
            {
                DeviceModel model = mapper.Map<DeviceEntity, DeviceModel>(entity);
                
                return model;
            }

            return null;
        }
    }
}