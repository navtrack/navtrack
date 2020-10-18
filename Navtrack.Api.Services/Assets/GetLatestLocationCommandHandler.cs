using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.CommandHandler;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Assets
{
    [Service(typeof(ICommandHandler<GetLatestLocationCommand, LocationResponseModel>))]
    public class GetLatestLocationCommandHandler : BaseCommandHandler<GetLatestLocationCommand, LocationResponseModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetLatestLocationCommandHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public override async Task<LocationResponseModel> Handle(GetLatestLocationCommand command)
        {
            LocationEntity location = await repository.GetEntities<LocationEntity>()
                .Where(x => x.AssetId == command.AssetId && x.Asset.Users.Any(y => y.UserId == command.UserId))
                .OrderByDescending(x => x.DateTime)
                .FirstOrDefaultAsync();

            if (location != null)
            {
                LocationResponseModel mapped = mapper.Map<LocationEntity, LocationResponseModel>(location);

                return mapped;
            }

            return null;
        }
    }
}