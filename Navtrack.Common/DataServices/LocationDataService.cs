using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;

namespace Navtrack.Common.DataServices
{
    [Service(typeof(ILocationDataService))]
    public class LocationDataService : ILocationDataService
    {
        private readonly IRepository repository;

        public LocationDataService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task Add(Location location)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();
            
            unitOfWork.Add(location);

            await unitOfWork.SaveChanges();
        }

        public async Task AddRange(IEnumerable<Location> locations)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();
            
            unitOfWork.AddRange(locations);

            await unitOfWork.SaveChanges();
        }
    }
}