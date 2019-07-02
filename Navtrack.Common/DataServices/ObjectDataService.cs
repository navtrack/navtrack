using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;

namespace Navtrack.Common.DataServices
{
    [Service(typeof(IObjectDataService))]
    public class ObjectDataService : IObjectDataService
    {
        private readonly IRepository repository;

        public ObjectDataService(IRepository repository)
        {
            this.repository = repository;
        }

        public Task<Object> GetObjectByIMEI(string imei)
        {
            return repository.GetEntities<Object>()
                .Include(x => x.Device)
                .FirstOrDefaultAsync(x => x.Device.IMEI == imei);
        }
    }
}