using Domain.Products;
using Infra.DAL.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nascimento.Software.Erp.Api.Dto.Products;
using Nascimento.Software.Erp.Api.Dto.RequestResponse;

namespace Nascimento.Software.Erp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryDAL _categoryDAL;

        public CategoryController(ICategoryDAL categoryDAL)
        {
            _categoryDAL = categoryDAL;
        }

        [HttpGet]
        [Route("get-categories")]
        public async Task<ActionResult> GetCategoriesAsync()
        {
            return Ok(await _categoryDAL.GetCategories());
        }
        [HttpGet]
        [Route("get-categories-by-id")]
        public async Task<ActionResult> GetCategoriesById([FromHeader] int id)
        {
            return Ok(await _categoryDAL.Get(id));
        }
        [HttpGet]
        [Route("get-categories-by-id-with-products")]
        public async Task<ActionResult> GetCagoriesWithProductsById([FromHeader] int Id)
        {
            return Ok(await _categoryDAL.GetCategoryWithProducts(Id));
        }
        [HttpPost]
        [Route("insert-new-category")]
        public async Task<ActionResult> InsertNewCategoryAsync([FromBody] CategoryDto categoryDto)
        {
            var request = new RequestResponseDto();
            var category = new Category() { IsActive = categoryDto.IsActive, Name = categoryDto.Name };
            var inserted = await _categoryDAL.Create(category);
            if (inserted)
            {
                request.Success = true;
                request.Messages.Add("Operation completed sucessfuly!");
                return Ok(request);
            }

            request.Success = false;
            request.Messages.Add("Internal server error");
            return BadRequest(request);
        }
        [HttpDelete]
        [Route("delete-category")]
        public async Task<ActionResult> DeleteCategoryByIdAsync([FromHeader] int Id)
        {
            var request = new RequestResponseDto();

            var categoryExists = await _categoryDAL.Get(Id);
            if (categoryExists == null)
            {
                request.Messages.Add("The category you want to delete don't exists!");
                return BadRequest(request);
            }

            var deleted = await _categoryDAL.Delete(categoryExists);
            if (deleted)
            {
                request.Success = true;
                request.Messages.Add("Deleted sucessfuly");
                return Ok(request);
            }

            request.Messages.Add("Internal server error when deleting.");
            return BadRequest(request);
        }
        [HttpPut]
        [Route("update-category")]
        public async Task<ActionResult> UpdateCategoryByIdAsync([FromHeader] int Id, [FromBody] CategoryDto categoryDto)
        {
            var request = new RequestResponseDto();
            var categoryExists = await _categoryDAL.Get(Id);
            if (categoryExists == null)
            {
                request.Messages.Add("The category you want to update don't exists!");
                return BadRequest(request);
            }
            var categoryUpdate = new Category()
            {
                Id = categoryExists.Id,
                IsActive = categoryDto.IsActive,
                Name = categoryDto.Name,
            };
            var updated = await _categoryDAL.Update(categoryUpdate);
            if (updated)
            {
                request.Success = true;
                request.Messages.Add("Updated sucessfuly");
                return Ok(request);
            }

            request.Messages.Add("Internal server error");
            return BadRequest(request);
        }
    }
}
