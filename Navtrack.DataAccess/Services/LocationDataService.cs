using System.Threading.Tasks;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Services
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
    }
}