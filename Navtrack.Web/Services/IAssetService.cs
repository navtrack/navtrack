using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Navtrack.Web.Models;

namespace Navtrack.Web.Services
{
    public interface IAssetService
    {
        Task<List<AssetModel>> GetAll();
        Task<AssetModel> Get(int id);
        Task ValidateModel(AssetModel asset, ModelStateDictionary modelState);
        Task Add(AssetModel asset);
    }
}