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
    public class ProductsController : ControllerBase
    {
        private readonly IProductsDAL _productsDAL;
        private readonly ICategoryDAL _categoryDAL;

        public ProductsController(IProductsDAL productsDAL, ICategoryDAL _categoryDAL)
        {
            this._categoryDAL = _categoryDAL;
            _productsDAL = productsDAL;
        }
        [HttpGet]
        [Route("get-all-products")]
        public async Task<ActionResult> GetProductsAsync()
        {
            return Ok(await _productsDAL.GetProducts());
        }
        [HttpGet]
        [Route("get-top-50-products")]
        public async Task<ActionResult> GetTop50Products()
        {
            return Ok(await _productsDAL.GetTop50Products());
        }
        [HttpGet]
        [Route("get-products-by-category")]
        public async Task<ActionResult> GetProductsByCategoryId([FromHeader] int Id)
        {
            return Ok(await _productsDAL.GetProductsByCategoryId(Id));
        }
        [HttpGet]
        [Route("get-product-by-id")]
        public async Task<ActionResult> GetProductById([FromHeader] int Id)
        {
            return Ok(await _productsDAL.GetProductById(Id));
        }
        [HttpPost]
        [Route("insert-new-product")]
        public async Task<ActionResult> InsertProductAsync([FromBody] ProductDto productDto)
        {
            var requestResponse = DtoValidator(productDto);
            if (!requestResponse.Success) return BadRequest(requestResponse);

            if (await _categoryDAL.Get(productDto.CategoryId) == null)
            {
                requestResponse.AddErrorMessage("category don't exists!");
                return BadRequest(requestResponse);
            }

            var product = new Product()
            {
                CategoryId = productDto.CategoryId,
                IsActive = productDto.IsActive,
                Name = productDto.Name,
                Value = productDto.Value,
            };

            if (await _productsDAL.Create(product))
            {
                requestResponse.Success = true;
                requestResponse.AddSucessMessage();
                return Ok(requestResponse);
            }

            return BadRequest(requestResponse);
        }
        [HttpPut]
        [Route("update-product")]
        public async Task<ActionResult> UpdateProduct([FromBody] ProductDto productDto, [FromHeader] int Id)
        {
            var requestResponse = DtoValidator(productDto);
            if (!requestResponse.Success) return BadRequest(requestResponse);

            var productExists = await _productsDAL.Get(Id);
            if (productExists == null)
            {
                requestResponse.AddErrorMessage("Product don't exists");
                return BadRequest(requestResponse);
            }
            if (await _categoryDAL.Get(productDto.CategoryId) == null)
            {
                requestResponse.AddErrorMessage("category don't exists");
                return BadRequest(requestResponse);
            }
            var product = new Product()
            {
                Id = Id,
                CategoryId = productDto.CategoryId,
                IsActive = productDto.IsActive,
                Name = productDto.Name,
                Value = productDto.Value,
            };

            if (await _productsDAL.Update(product))
            {
                requestResponse.AddSucessMessage();
                return Ok(requestResponse);
            }

            requestResponse.AddErrorMessage("Internal server error");
            return BadRequest(requestResponse);

        }
        private RequestResponseDto DtoValidator(ProductDto productdto)
        {
            var requestResponse = new RequestResponseDto();

            if (string.IsNullOrWhiteSpace(productdto.Name)) requestResponse.AddErrorMessage("Name empty or null");
            if (productdto.Value < 0) requestResponse.AddErrorMessage("Value negative");

            if (requestResponse.Messages.Count == 0)
            {
                requestResponse.Success = true;
            }
            return requestResponse;
        }
    }
}