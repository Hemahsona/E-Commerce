using E_Commerce.Application.Common;
using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs;
using E_Commerce.Application.Services;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contract.IRepositories;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Services
{
    internal class ProductService(IUnitOfWork unitOfWork, IUrlService urlService) : IProductService
    {
        public async Task<Result<PaginatedResult<ProductDTO>>> GetAllAsync(ProductQueryPramas queryPramas, CancellationToken ct)
        {
            var spec = new ProductWithTypeAndBrandSpec(queryPramas);
            var products = await unitOfWork.Repository<Product, int>().GetAllAsync(spec, ct);
            var data = products.Select(product => new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = urlService.GetImageUrl(product.PictureUrl),
                Price = product.Price,
                ProductBrand = product.Brand?.Name,
                ProductType = product.Type?.Name,
            }).ToList();
            var countSpec = new ProductCountSpecifications(queryPramas);
            var countAllProducts = await unitOfWork.Repository<Product, int>().CountAsync(countSpec, ct);
            var result = new PaginatedResult<ProductDTO>(queryPramas.PageIndex, queryPramas.PageSize, countAllProducts, data);        
            return Result<PaginatedResult<ProductDTO>>.Ok(result);
        }
        

        public async Task<Result<IReadOnlyList<BrandDTOs>>> GetAllBrandsAsync(CancellationToken ct)
        {
            var brands = await unitOfWork.Repository<ProductBrand, int>().GetAllAsync(ct);
            var result = brands.Select(brand => new BrandDTOs
            {
                Id = brand.Id,
                Name = brand.Name,
            }).ToList();
            return Result<IReadOnlyList<BrandDTOs>>.Ok(result);
        }
        

        public async Task<Result<IReadOnlyList<TypeDTOs>>> GetAllTypesAsync(CancellationToken ct)
        {
            var types = await unitOfWork.Repository<ProductType, int>().GetAllAsync(ct);
            var result = types.Select(type => new TypeDTOs
            {
                Id = type.Id,
                Name = type.Name,
            }).ToList();
            return Result<IReadOnlyList<TypeDTOs>>.Ok(result);
        }

        public async Task<Result<ProductDTO>> GetByIdAsync(int id, CancellationToken ct)
        {
            var spec = new ProductWithTypeAndBrandSpec(id);
            var product = await unitOfWork.Repository<Product, int>().GetByIdAsync(spec, ct);
            if (product == null)
                return Error.NotFound("Product Not Found", $"Product with id {id} Is not Found");
            var result = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.Brand.Name,
                ProductType = product.Type.Name,
            };

            return result;
        }
    }
}
