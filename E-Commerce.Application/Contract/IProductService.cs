using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contract
{
    public interface IProductService
    {
        Task<Result<PaginatedResult<ProductDTO>>> GetAllAsync(ProductQueryPramas queryPramas, CancellationToken ct);
        Task<Result<ProductDTO>> GetByIdAsync(int id, CancellationToken ct);
        Task<Result<IReadOnlyList<BrandDTOs>>> GetAllBrandsAsync(CancellationToken ct);
        Task<Result<IReadOnlyList<TypeDTOs>>> GetAllTypesAsync(CancellationToken ct);
    }
}
