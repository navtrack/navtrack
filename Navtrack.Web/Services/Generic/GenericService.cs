using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.Services;

namespace Navtrack.Web.Services.Generic
{
    public class GenericService<TEntity, TModel> : IGenericService<TEntity, TModel> where TEntity : class, IEntity
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        protected GenericService(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<TModel> Get(int id)
        {
            TEntity entity = await GetQueryable().FirstOrDefaultAsync(x => x.Id == id);

            return entity != null
                ? mapper.Map<TEntity, TModel>(entity)
                : default;
        }
        
        public async Task<List<TModel>> GetAll()
        {
            List<TEntity> devices = await GetQueryable().ToListAsync();

            List<TModel> mapped = devices.Select(mapper.Map<TEntity, TModel>).ToList();

            return mapped;
        }

        public async Task Add(TModel model)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

            TEntity mapped = mapper.Map<TModel, TEntity>(model);

            unitOfWork.Add(mapped);

            await unitOfWork.SaveChanges();
        }

        public async Task Update(TModel model)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

            TEntity mapped = mapper.Map<TModel, TEntity>(model);

            unitOfWork.Update(mapped);

            await unitOfWork.SaveChanges();
        }

        public async Task Delete(int id)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

            TEntity entity = await unitOfWork.GetEntities<TEntity>().FirstOrDefaultAsync(x => x.Id == id);

            unitOfWork.Delete(entity);

            await unitOfWork.SaveChanges();
        }

        public Task<bool> Exists(int id)
        {
            return repository.GetEntities<TEntity>().AnyAsync(x => x.Id == id);
        }

        public async Task<ValidationResult> ValidateSave(TModel model)
        {
            ValidationResult validationResult = new ValidationResult();

            await ValidateSave(model, validationResult);
            
            return validationResult;
        }

        protected virtual Task ValidateSave(TModel model, ValidationResult validationResult)
        {
            return Task.CompletedTask;
        }

        public async Task<ValidationResult> ValidateDelete(int id)
        {
            ValidationResult validationResult = new ValidationResult();

            await ValidateDelete(id, validationResult);
            
            return validationResult;
        }

        protected virtual Task ValidateDelete(int id, ValidationResult validationResult)
        {
            return Task.CompletedTask;
        }


        protected virtual IQueryable<TEntity> GetQueryable()
        {
            return repository.GetEntities<TEntity>();
        }
    }
}