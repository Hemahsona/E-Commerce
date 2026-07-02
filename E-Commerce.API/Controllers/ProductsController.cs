using Microsoft.AspNetCore.Mvc;

using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs;
using E_Commerce.Application.Common;

namespace E_Commerce.API.Controllers
{

    public class ProductsController(IProductService product) : ApiBaseController
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAll([FromQuery] ProductQueryPramas queryPramas, CancellationToken ct)
        {
            var result = await product.GetAllAsync(queryPramas, ct);
            return TOActionResult(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDTO>> GetById(int id, CancellationToken ct)
        {
            var result = await product.GetByIdAsync(id, ct);
            return TOActionResult(result);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDTOs>>> GetAllBrands(CancellationToken ct) 
        {
            var result = await product.GetAllBrandsAsync(ct);
            return TOActionResult(result);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<TypeDTOs>>> GetAllTypes(CancellationToken ct) 
        {
            var result = await product.GetAllTypesAsync(ct);
            return TOActionResult(result);
        }
    }
}
