using E_Commerce.Application.DTOs;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace E_Commerce.Application.Mapping
{
    internal static class ProductMappingExtension
    {
        public static ProductDTO ToProductDTO(this Product product) 
        {
            return new ProductDTO
            {
                Id = product.id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.Brand.Name,
                ProductType = product.Type.Name,
            };
        }
    }
}
