using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Web.Models;
using Navtrack.Web.Services.Generic;

namespace Navtrack.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
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
        public async Task<IActionResult> Get(int id)
        {
            if (await genericService.Exists(id))
            {
                if (await genericService.Authorize(id, EntityRole.Viewer))
                {
                    TModel model = await genericService.Get(id);

                    return Ok(model);
                }

                return Unauthorized();
            }

            return NotFound();
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(TModel model)
        {
            if (ModelState.IsValid)
            {
                if (await genericService.Exists(model.Id))
                {
                    if (await genericService.Authorize(model.Id, EntityRole.Owner))
                    {
                        ValidationResult validationResult = await genericService.ValidateSave(model);

                        if (validationResult.IsValid)
                        {
                            await genericService.Update(model);

                            return Ok();
                        }

                        return BadRequest(new ErrorModel(validationResult));
                    }

                    return Unauthorized();
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
                if (await genericService.Authorize(id, EntityRole.Owner))
                {
                    ValidationResult validationResult = await genericService.ValidateDelete(id);

                    if (validationResult.IsValid)
                    {
                        await genericService.Delete(id);

                        return Ok();
                    }

                    return BadRequest(new ErrorModel(validationResult));
                }

                return Unauthorized();
            }

            return NotFound();
        }
    }
}