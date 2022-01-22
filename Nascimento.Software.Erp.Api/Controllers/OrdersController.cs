using Domain.Orders;
using Infra.DAL.Contracts;
using Microsoft.AspNetCore.Mvc;
using Nascimento.Software.Erp.Api.Dto.Orders;
using Nascimento.Software.Erp.Api.Dto.RequestResponse;

namespace Nascimento.Software.Erp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderDAL _orderDAL;
        private readonly IBuyerDAL _buyerDAL;
        private readonly IProductsDAL _productDAL;
        public OrdersController(IOrderDAL orderDal, IBuyerDAL buyerDAL, IProductsDAL productDAL)
        {
            _productDAL = productDAL;
            _orderDAL = orderDal;
            _buyerDAL = buyerDAL;

        }
        [Route("place-order")]
        [HttpPost]
        public async Task<ActionResult> PlaceOrderAsync([FromBody] OrderDto orderDto)
        {
            var requestResponse = await DtoValidator(orderDto);
            if (requestResponse.Success is false) return BadRequest(requestResponse);

            if (await _buyerDAL.Get(orderDto.BuyerId) is null)
            {
                requestResponse.AddErrorMessage("Buyer Id don't exists!");
                return BadRequest(requestResponse);
            }

            var listProductOrder = new List<Place_Order_Product>();
            foreach (var item in orderDto.Products)
            {
                listProductOrder.Add(new Place_Order_Product()
                {
                    Id = item.Id,
                    Product_Quantity = item.Quantity
                });
            }
            var order = new PlaceOrder()
            {
                BuyerId = orderDto.BuyerId,
                Products = listProductOrder,
                TotalValue = await SetTotalValue(orderDto),
            };
            var inserted = await _orderDAL.PlaceOrder(order);
            return Ok();
        }
        [HttpGet]
        [Route("get-orders-by-buyer")]
        public async Task<ActionResult> GetOrdersByBuyerAsync([FromHeader] int BuyerId)
        {
            return Ok(await _orderDAL.GetOrdersByBuyerId(BuyerId));
        }
        [HttpGet]
        [Route("get-order-by-id")]
        public async Task<ActionResult> GetOrderByIdAsync([FromHeader] int orderId)
        {
            return Ok();
        }
        [HttpGet]
        [Route("get-all-orders")]
        public async Task<ActionResult> GetOrdersAsync()
        {
            return Ok(await _orderDAL.GetAllOrders());
        }

        private async Task<RequestResponseDto> DtoValidator(OrderDto orderDto)
        {
            var requestResponse = new RequestResponseDto();

            if (orderDto.Products.Count <= 0) requestResponse.AddErrorMessage("Product list empty");

            foreach (var item in orderDto.Products)
            {
                if (await _productDAL.Get(item.Id) is null) requestResponse.AddErrorMessage("Product don't exists!");
            }

            if (requestResponse.Messages.Count < 1) requestResponse.Success = true;

            return requestResponse;
        }

        private async Task<decimal> SetTotalValue(OrderDto orderDto)
        {
            var totalValue = 0.0M;
            foreach (var item in orderDto.Products)
            {
                var product = await _productDAL.Get(item.Id);
                if (product != null)
                {
                    totalValue = +(product.Value * item.Quantity);
                }
            }
            return totalValue;
        }
    }
}
