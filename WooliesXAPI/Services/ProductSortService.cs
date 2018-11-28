using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WooliesXAPI.Common;
using WooliesXAPI.Models;

namespace WooliesXAPI.Services
{
    public interface IProductSortService
    {
        List<Product> SortProductListBy(List<Product> products, ProductSortOrder sortOrder);
    }
    public class ProductSortService : IProductSortService
    {
        private readonly ILogger<ProductSortService> _logger;
        public ProductSortService(ILogger<ProductSortService> logger)
        {
            _logger = logger;
        }

        public List<Product> SortProductListBy(List<Product> products, ProductSortOrder sortOrder)
        {
            try
            {
                switch (sortOrder)
                {
                    case ProductSortOrder.Low:
                        return products.OrderBy(c => c.Price).ToList();
                    case ProductSortOrder.High:
                        return products.OrderByDescending(c => c.Price).ToList();
                    case ProductSortOrder.Ascending:
                    case ProductSortOrder.DefaultSortOrder:
                        return products.OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase).ToList();
                    case ProductSortOrder.Descending:
                        return products.OrderByDescending(p => p.Name, StringComparer.OrdinalIgnoreCase).ToList();
                    case ProductSortOrder.Recommended:
                        return products
                            .GroupBy(p => new {p.Name})
                            .Select(g => new Product {Name = g.Key.Name, Quantity = g.Sum(p => p.Quantity)})
                            .OrderByDescending(c => c.Quantity).Select(r =>
                                new Product {Name = r.Name}).Distinct()
                            .ToList();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(sortOrder), sortOrder, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Sorting Product list {sortOrder.ToString()}");
                throw;
            }
        }
    }
}
