using System.Threading.Tasks;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;

namespace Navtrack.Listener.Services.Logging
{
    [Service(typeof(ILogger))]
    public class Logger : ILogger
    {
        private readonly IRepository repository;

        public Logger(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task Log(string data)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();
            
            Log log = new Log
            {
                Data = data
            };

            unitOfWork.Add(log);

            await unitOfWork.SaveChanges();
        }
    }
}