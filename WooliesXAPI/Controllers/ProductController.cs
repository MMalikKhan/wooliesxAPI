using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using WooliesXAPI.Common;
using WooliesXAPI.ExternalHttpClientServices;
using WooliesXAPI.Models;
using WooliesXAPI.Services;

namespace WooliesXAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
       
        private readonly IWooliesXHttpClient _wooliesXHttpClient;
        private readonly IProductSortService _productSortService;
        private readonly ILogger<ProductController> _logger;
       

        public ProductController(IWooliesXHttpClient wooliesXHttpClient, IProductSortService productSortService,
            ILogger<ProductController> logger)
        {
            _wooliesXHttpClient = wooliesXHttpClient;
            _productSortService = productSortService;
            _logger = logger;
        }

        [HttpGet]
        [Route("sort")]
        public async Task<ActionResult<List<Product>>> Sort([BindRequired, FromQuery] string sortOption)
        {
            _logger.LogTrace($"api/product/sort executing sortOption {sortOption}");
            if (string.IsNullOrWhiteSpace(sortOption) || !ModelState.IsValid)
            {
                BadRequest("Not all required information was supplied");
            }

            Enum.TryParse(sortOption, out ProductSortOrder productSortOrder);

            var sortedProducts = await GetProductSortedListAsync(productSortOrder);
            _logger.LogTrace($"api/product/sort products sorted for option {sortOption}");
            return Ok(sortedProducts);
        }

        private async Task<List<Product>> GetProductSortedListAsync(ProductSortOrder productSortOrder)
        {
            var productsList = await _wooliesXHttpClient.GetProductListAsync();
            if (productSortOrder != ProductSortOrder.Recommended)
            {   
                return _productSortService.SortProductListBy(productsList, productSortOrder);
            }
            else
            {
                //get shopper history
                var shopperHistory = await _wooliesXHttpClient.GetShopperHistoryProductListAsync();
                //select all products from alll customers
                var products = shopperHistory.SelectMany(x => x.Products).ToList();
                var sortedRecommendedList = _productSortService.SortProductListBy(products, productSortOrder);
                var sortedProductsList = new List<Product>();

                foreach (var recommendedProduct in sortedRecommendedList)
                {
                    var product = productsList.FirstOrDefault(p => p.Name == recommendedProduct.Name);
                    if (product != null)
                    {
                        sortedProductsList.Add(product);
                        //remove products which are added to the sorted list
                        productsList.Remove(product);
                    }
                }
                //add all remain products from actual productlist retreived
                foreach (var product in productsList)
                {
                    sortedProductsList.Add(product);
                }

                return sortedProductsList;
            }
        }


        [HttpGet]
        [Route("trolleycalculations")]
        public async Task<ActionResult> GetTrolleyCalculationsAsync()
        {
            var result = await _wooliesXHttpClient.GetTrolleyCalculator();
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await _wooliesXHttpClient.GetProductListAsync();
            return Ok(result);
        }
    }
}