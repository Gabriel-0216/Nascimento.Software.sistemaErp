using Domain.Buyers;
using Infra.DAL.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nascimento.Software.Erp.Api.Dto.Buyer;
using Nascimento.Software.Erp.Api.Dto.RequestResponse;

namespace Nascimento.Software.Erp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BuyersController : ControllerBase
    {
        private readonly IBuyerDAL _buyerDAL;
        public BuyersController(IBuyerDAL _buyerDAL)
        {
            this._buyerDAL = _buyerDAL;
        }
        [HttpGet]
        [Route("get-buyers-with-orders")]
        public async Task<ActionResult> GetBuyersWithOrdersAsync([FromHeader] int Id)
        {
            return Ok(await _buyerDAL.GetBuyerWithOrder(Id));
        }
        [HttpPost]
        [Route("insert-new-buyer")]
        public async Task<ActionResult> InsertNewBuyerAsync([FromBody] BuyerDto buyerDto)
        {
            var requestResponse = DtoValidation(buyerDto);
            if (!requestResponse.Success) return BadRequest(requestResponse);

            var buyer = new Buyer()
            {
                Cellphone = buyerDto.Cellphone,
                Email = buyerDto.Email,
                FirstName = buyerDto.FirstName,
                IsActive = buyerDto.IsActive,
                LastName = buyerDto.LastName,
            };
            var inserted = await _buyerDAL.Create(buyer);
            if (inserted) return Ok(buyerDto);

            requestResponse.AddErrorMessage("Internal server error, insert failed");
            return BadRequest(requestResponse);
        }
        [HttpPut]
        [Route("update-buyer-register")]
        public async Task<ActionResult> UpdateBuyerRegister([FromBody] BuyerDto buyerDto, [FromHeader] int Id)
        {
            var requestResponse = DtoValidation(buyerDto);
            if (!requestResponse.Success) return BadRequest(requestResponse);

            var buyerExists = await _buyerDAL.Get(Id);
            if (buyerExists == null)
            {
                requestResponse.AddErrorMessage("User don't exists");
                return BadRequest(requestResponse);
            }

            var buyer = new Buyer()
            {
                Id = buyerExists.Id,
                Cellphone = buyerDto.Cellphone,
                Email = buyerDto.Email,
                FirstName = buyerDto.FirstName,
                IsActive = buyerDto.IsActive,
                LastName = buyerDto.LastName,
            };
            var updated = await _buyerDAL.Update(buyer);
            requestResponse.AddSucessMessage();
            if (updated) return Ok(requestResponse);

            requestResponse.AddErrorMessage("Failed updated, internal server error");
            return BadRequest(requestResponse);
        }
        [HttpGet]
        [Route("get-buyer-with-email")]
        public async Task<ActionResult> GetBuyerWithEmailAsync([FromHeader] string Email)
        {
            return Ok(await _buyerDAL.GetBuyerWithEmail(Email));
        }
        [HttpGet]
        [Route("get-buyer-with-username")]
        public async Task<ActionResult> GetBuyerWithUsernameAsync([FromHeader] string Username)
        {
            return Ok(await _buyerDAL.GetBuyerWithUserName(Username));
        }
        [HttpGet]
        [Route("get-buyer-with-cellphone")]
        public async Task<ActionResult> GetBuyerWithCellphoneAsync([FromHeader] string Cellphone)
        {
            return Ok(await _buyerDAL.GetBuyerWithCellphone(Cellphone));
        }
        [HttpGet]
        [Route("get-all-buyers")]
        public async Task<ActionResult> GetBuyersAsync()
        {
            return Ok(await _buyerDAL.GetAll());
        }
        [HttpGet]
        [Route("get-buyers-by-id")]
        public async Task<ActionResult> GetBuyersByIdAsync([FromHeader] int Id)
        {
            return Ok(await _buyerDAL.Get(Id));
        }
        private RequestResponseDto DtoValidation(BuyerDto buyerDto)
        {
            var request = new RequestResponseDto();
            request.Success = false;
            if (string.IsNullOrWhiteSpace(buyerDto.FirstName)) request.AddErrorMessage("The first name cannot be null or empty");
            if (string.IsNullOrWhiteSpace(buyerDto.LastName)) request.AddErrorMessage("The last name cannot be null or empty");
            if (string.IsNullOrWhiteSpace(buyerDto.Cellphone)) request.AddErrorMessage("The cellphone  cannot be null or empty");
            if (string.IsNullOrWhiteSpace(buyerDto.Email)) request.AddErrorMessage("The email cannot be null or empty");

            if (request.Messages.Count == 0) request.Success = true;

            return request;
        }
    }
}