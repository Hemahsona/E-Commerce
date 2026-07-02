using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Specifications
{
    internal class ProductCountSpecifications : BaseSpecifications<Product, int>
    {
        public ProductCountSpecifications(ProductQueryPramas queryPramas) : base(p => (!queryPramas.BrandId.HasValue || p.BrandId == queryPramas.BrandId)
            && (!queryPramas.TypeId.HasValue || p.TypeId == queryPramas.TypeId)
            && (string.IsNullOrWhiteSpace(queryPramas.Name) || p.Name.ToLower().Contains(queryPramas.Name.ToLower())))
        {
            
        }
    }
}
