using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs.Baskets;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class BasketsController(IBasketService basket) : ApiBaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<BasketDto>> GetById(string id, CancellationToken ct)
        {
            var result = await basket.GetAsync(id, ct);
            return TOActionResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdate(BasketDto basketDto, CancellationToken ct)
        {
            var result = await basket.CreateOrUpdateAsync(basketDto, ct: ct);
            return TOActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(string id, CancellationToken ct)
        {
            var result = await basket.DeleteAsync(id, ct);
            return TOActionResult(result);
        }
    }
}
