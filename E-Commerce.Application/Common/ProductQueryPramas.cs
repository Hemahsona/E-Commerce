using E_Commerce.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Common
{
    public class ProductQueryPramas
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Name { get; set; }
        public ProductSortingOptions Sort { get; set; }
        public int PageIndex { get; set; } = 1;

        private const int defaultPageSize = 5;

        private const int MaxPageSize = 10;

        private int _pageSize = defaultPageSize;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : (value < 1 ? defaultPageSize : value);
        }
    }
}
