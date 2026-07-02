using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Specifications
{
    internal class ProductWithTypeAndBrandSpec : BaseSpecifications<Product, int>
    {
        public ProductWithTypeAndBrandSpec(ProductQueryPramas queryPramas)
            : base(p => (!queryPramas.BrandId.HasValue || p.BrandId==queryPramas.BrandId)
            && (!queryPramas.TypeId.HasValue || p.TypeId==queryPramas.TypeId)
            && (string.IsNullOrWhiteSpace(queryPramas.Name) || p.Name.ToLower().Contains(queryPramas.Name.ToLower())))
        {
            AddInclude(p => p.Brand);
            AddInclude(p => p.Type);
            switch(queryPramas.Sort)
            {
                case Enums.ProductSortingOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case Enums.ProductSortingOptions.NameDesc:
                    AddOrderByDesc(p => p.Name);
                    break;
                case Enums.ProductSortingOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case Enums.ProductSortingOptions.PriceDesc:
                    AddOrderByDesc(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Id);
                    break;
            }
            ApplyPagination(queryPramas.PageSize, queryPramas.PageIndex);
        }
        public ProductWithTypeAndBrandSpec(int id):base(p => p.Id == id)
        {
            AddInclude(p => p.Brand);
            AddInclude(p => p.Type);
            
        }

    }
}
