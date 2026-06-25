using E_Commerce.Application.Common;
using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs;
using E_Commerce.Application.Services;
using E_Commerce.Domain.Contract.IRepositories;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Services
{
    internal class ProductService(IUnitOfWork unitOfWork) : IProductService
    {
        public async Task<Result<IEnumerable<ProductDTO>>> GetAllAsync(CancellationToken ct)
        {
            var products = await unitOfWork.Repository<Product, int>().GetAllAsync(ct);
            var result = products.Select(product => new ProductDTO
            {
                Id = product.id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.Brand.Name,
                ProductType = product.Type.Name,
            });
            return Result<IEnumerable<ProductDTO>>.Ok(result);
        }
        

        public async Task<Result<IEnumerable<BrandDTOs>>> GetAllBrandsAsync(CancellationToken ct)
        {
            var brands = await unitOfWork.Repository<ProductBrand, int>().GetAllAsync(ct);
            var result = brands.Select(brand => new BrandDTOs
            {
                Id = brand.id,
                Name = brand.Name,
            });
            return Result<IEnumerable<BrandDTOs>>.Ok(result);
        }
        

        public async Task<Result<IEnumerable<TypeDTOs>>> GetAllTypesAsync(CancellationToken ct)
        {
            var types = await unitOfWork.Repository<ProductType, int>().GetAllAsync(ct);
            var result = types.Select(type => new TypeDTOs
            {
                Id = type.id,
                Name = type.Name,
            });
            return Result<IEnumerable<TypeDTOs>>.Ok(result);
        }

        public async Task<Result<ProductDTO>> GetByIdAsync(int id, CancellationToken ct)
        {
            var product = await unitOfWork.Repository<Product, int>().GetByIdAsync(id, ct);
            if (product == null)
                return Error.NotFound("Product Not Found", $"Product with id {id} Is not Found");
            var result = new ProductDTO
            {
                Id = product.id,
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
