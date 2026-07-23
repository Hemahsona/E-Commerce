using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_Commerce.Application.Specifications
{
    internal class ProductWithSpecifications : BaseSpecifications<Product, int>
    {
        public ProductWithSpecifications(HashSet<int> ProductIds) : base(p => ProductIds.Contains(p.Id))
        {
        }
    }
}
