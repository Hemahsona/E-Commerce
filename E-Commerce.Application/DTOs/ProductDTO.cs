using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public string ProductBrand { get; set; }
        public string ProductType { get; set; }
    }
}
