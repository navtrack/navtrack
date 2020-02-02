using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.Services;
using Navtrack.Web.Models;
using Navtrack.Web.Services.Extensions;

namespace Navtrack.Web.Services.Generic
{
    public class GenericService<TEntity, TModel> : IGenericService<TEntity, TModel>
        where TEntity : class, IEntity where TModel : class, IModel
    {
        protected readonly IRepository repository;
        protected readonly IMapper mapper;
        protected readonly IHttpContextAccessor httpContextAccessor;

        protected GenericService(IRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<TModel> Get(int id)
        {
            TEntity entity = await GetQueryable().FirstOrDefaultAsync(x => x.Id == id);

            TModel model = MapToModel(entity);

            return model;
        }

        public virtual async Task<List<TModel>> GetAll()
        {
            List<TEntity> entities = await GetQueryable().ToListAsync();

            List<TModel> models = entities.Select(MapToModel).ToList();

            return models;
        }

        public async Task Add(TModel model)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

            TEntity entity = MapToEntity(model);

            await BeforeAdd(unitOfWork, model, entity);

            unitOfWork.Add(entity);

            await unitOfWork.SaveChanges();
        }

        public async Task Update(TModel model)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

            TEntity entity = await repository.GetEntities<TEntity>().FirstOrDefaultAsync(x => x.Id == model.Id);

            mapper.Map(model, entity);

            unitOfWork.Update(entity);

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

        public async Task<bool> Authorize(int id, Role role)
        {
            IUserRelation userRelation = await GetUserRelation(id);

            return userRelation == null ||
                   userRelation.UserId == httpContextAccessor.HttpContext.User.GetId() &&
                   userRelation.RoleId == (int) role ||
                   role == Role.Viewer && userRelation.RoleId == (int) Role.Owner; // TODO change this
        }

        protected virtual Task<IUserRelation> GetUserRelation(int id)
        {
            return null;
        }

        protected TModel MapToModel(TEntity entity)
        {
            return entity != null ? mapper.Map<TEntity, TModel>(entity) : default;
        }

        protected TEntity MapToEntity(TModel model)
        {
            return model != null ? mapper.Map<TModel, TEntity>(model) : default;
        }

        protected virtual Task BeforeAdd(IUnitOfWork unitOfWork, TModel model, TEntity entity)
        {
            return Task.CompletedTask;
        }
    }
}