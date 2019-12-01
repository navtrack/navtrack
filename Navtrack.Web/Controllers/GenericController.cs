using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Web.Models;
using Navtrack.Web.Services.Generic;

namespace Navtrack.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenericController<TEntity, TModel> : ControllerBase 
        where TEntity : class
        where TModel : IModel
    {
        private readonly IGenericService<TEntity, TModel> genericService;

        public GenericController(IGenericService<TEntity, TModel> genericService)
        {
            this.genericService = genericService;
        }

        [HttpGet("{id}")]
        public Task<TModel> Get(int id)
        {
            return genericService.Get(id);
        }

        [HttpGet]
        public Task<List<TModel>> GetAll()
        {
            return genericService.GetAll();
        }

        [HttpPost]
        public async Task<IActionResult> Add(TModel model)
        {
            if (ModelState.IsValid)
            {
                ValidationResult validationResult = await genericService.ValidateSave(model);

                if (validationResult.IsValid)
                {
                    await genericService.Add(model);

                    return Ok();
                }

                return BadRequest(new ErrorModel(validationResult));
            }

            return ValidationProblem();
        }

        [HttpPut]
        public async Task<IActionResult> Update(TModel model)
        {
            if (ModelState.IsValid)
            {
                if (await genericService.Exists(model.Id))
                {
                    ValidationResult validationResult = await genericService.ValidateSave(model);

                    if (validationResult.IsValid)
                    {
                        await genericService.Update(model);

                        return Ok();
                    }

                    return BadRequest(new ErrorModel(validationResult));
                }

                return NotFound();
            }

            return ValidationProblem();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await genericService.Exists(id))
            {
                ValidationResult validationResult = await genericService.ValidateDelete(id);

                if (validationResult.IsValid)
                {
                    await genericService.Delete(id);

                    return Ok();
                }

                return BadRequest(new ErrorModel(validationResult));
            }

            return NotFound();
        }
    }
}