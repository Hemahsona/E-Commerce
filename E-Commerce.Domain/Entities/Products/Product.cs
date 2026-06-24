using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Entities.Products
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; } = default!;
        public ProductType Type { get; set; } = default!;
        public int TypeId { get; set; }
        public ProductBrand Brand { get; set; } = default!;
        public int BrandId { get; set; }
    }
}
