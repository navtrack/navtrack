using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.RequestHandlers;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Locations
{
    [Service(typeof(ICommandHandler<GetLatestLocationCommand, LocationModel>))]
    public class GetLatestLocationCommandHandler : BaseCommandHandler<GetLatestLocationCommand, LocationModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetLatestLocationCommandHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public override async Task<LocationModel> Handle(GetLatestLocationCommand command)
        {
            LocationEntity location = await repository.GetEntities<LocationEntity>()
                .Where(x => x.AssetId == command.AssetId && x.Asset.Users.Any(y => y.UserId == command.UserId))
                .OrderByDescending(x => x.DateTime)
                .FirstOrDefaultAsync();

            if (location != null)
            {
                LocationModel mapped = mapper.Map<LocationEntity, LocationModel>(location);

                return mapped;
            }

            return null;
        }
    }
}