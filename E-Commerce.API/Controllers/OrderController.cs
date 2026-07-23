using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class OrderController(IOrderService orderService) : ApiBaseController
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderTOReturnDto>> Create(OrderDto orderDto, CancellationToken ct)
             => TOActionResult(await orderService.CreateOrderAsync(orderDto, GetCurrentUserEmail(), ct));

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderTOReturnDto>>> GetAll(CancellationToken ct)
             => TOActionResult(await orderService.GetAllForUserAsync(GetCurrentUserEmail(), ct));

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderTOReturnDto>> GetById(Guid id, CancellationToken ct)
             => TOActionResult(await orderService.GetByIdAndEmailForUserAsync(id, GetCurrentUserEmail(), ct));

        [HttpGet("DeliveyMethod")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethods(CancellationToken ct)
            => TOActionResult(await orderService.GetDeliveryMethodAsync(ct));

    }
}
