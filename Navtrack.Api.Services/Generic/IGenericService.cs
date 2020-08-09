// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Navtrack.Api.Model;
// using Navtrack.Api.Model.Models;
//
// namespace Navtrack.Api.Services.Generic
// {
//     public interface IGenericService<TEntity, TModel>
//     {
//         Task<TModel> Get(int id);
//         Task<List<TModel>> GetAll();
//         Task<ApiResponseModel> ValidateSave(TModel model);
//         Task Add(TModel model);
//         Task<bool> Exists(int id);
//         Task Update(TModel model);
//         Task<ApiResponseModel> ValidateDelete(int id);
//         Task Delete(int id);
//         Task<bool> Authorize(int id, EntityRole entityRole);
//     }
// }