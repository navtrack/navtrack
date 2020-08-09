using System.Threading.Tasks;
using Navtrack.DataAccess.Repository;

namespace Navtrack.DataAccess.Services
{
    public class GenericDataService<T> : IGenericDataService<T> where T : class
    {
        private readonly IRepository repository;

        protected GenericDataService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task Add(T entity)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

            unitOfWork.Add(entity);

            await unitOfWork.SaveChanges();
        }
    }
}