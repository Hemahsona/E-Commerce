using Microsoft.AspNetCore.Mvc;

using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs;
using E_Commerce.Application.Common;

namespace E_Commerce.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProductsController(IProductService product) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Result<IReadOnlyList<ProductDTO>>>> GetAll(CancellationToken ct)
        {
            var result = await product.GetAllAsync(ct);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Result<ProductDTO>>> GetById(int id, CancellationToken ct)
        {
            var result = await product.GetByIdAsync(id, ct);
            return Ok(result);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<Result<IEnumerable<BrandDTOs>>>> GetAllBrands(CancellationToken ct) 
        {
            var result = await product.GetAllBrandsAsync(ct);
            return Ok(result);
        }
        [HttpGet("types")]
        public async Task<ActionResult<Result<IEnumerable<TypeDTOs>>>> GetAllTypes(CancellationToken ct) 
        {
            var result = await product.GetAllTypesAsync(ct);
            return Ok(result);
        }
    }
}
