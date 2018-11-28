using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using WooliesXAPI.Common;
using WooliesXAPI.Controllers;
using WooliesXAPI.ExternalHttpClientServices;
using WooliesXAPI.Models;
using WooliesXAPI.Services;
using Xunit;

namespace WooliesXAPITest
{
    public class ProductControllerTest
    {
        [Fact]
        
        public async void CheckProductSortOptionByLowSuccess()
        {
            //Arrange
            var controller = MockProductControllerSetup();
            var expectedOrderedList = new List<string> {"C", "B", "F", "D", "A"};

            //Act
            var actionResult =  await controller.Sort(ProductSortOrder.Low.ToString());

            //Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var sortedProducts = okObjectResult.Value as List<Product>;
            Assert.NotNull(sortedProducts);
            for (int i = 0; i < sortedProducts.Count; i++)
            {
                Assert.True(sortedProducts[i].Name == expectedOrderedList[i]);
            }
        }

        [Fact]
        public async void CheckProductSortOptionByHighSuccess()
        {
            //Arrange
            var controller = MockProductControllerSetup();
            var expectedOrderedList = new List<string> { "A", "D", "F", "B", "C" };

            //Act
            var actionResult = await controller.Sort(ProductSortOrder.High.ToString());

            //Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var sortedProducts = okObjectResult.Value as List<Product>;
            Assert.NotNull(sortedProducts);
            for (int i = 0; i < sortedProducts.Count; i++)
            {
                Assert.True(sortedProducts[i].Name == expectedOrderedList[i]);
            }
        }

        [Fact]
        public async void CheckProductSortOptionByAscendingSuccess()
        {
            //Arrange
            var controller = MockProductControllerSetup();
            var expectedOrderedList = new List<string> { "A", "B", "C", "D", "F" };

            //Act
            var actionResult = await controller.Sort(ProductSortOrder.Ascending.ToString());

            //Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var sortedProducts = okObjectResult.Value as List<Product>;
            Assert.NotNull(sortedProducts);
            for (int i = 0; i < sortedProducts.Count; i++)
            {
                Assert.True(sortedProducts[i].Name == expectedOrderedList[i]);
            }
        }
        [Fact]
        public async void CheckProductSortOptionByDescendingSuccess()
        {
            //Arrange
            var controller = MockProductControllerSetup();
            var expectedOrderedList = new List<string> { "F", "D", "C", "B", "A" };

            //Act
            var actionResult = await controller.Sort(ProductSortOrder.Descending.ToString());

            //Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var sortedProducts = okObjectResult.Value as List<Product>;
            Assert.NotNull(sortedProducts);
            for (int i = 0; i < sortedProducts.Count; i++)
            {
                Assert.True(sortedProducts[i].Name == expectedOrderedList[i]);
            }
        }

        [Fact]
        public async void CheckProductSortOptionByRecommendedSuccess()
        {
            //Arrange
            var controller = MockProductControllerSetup();
            var expectedOrderedList = new List<string> { "C", "B", "A", "D", "F" };

            //Act
            var actionResult = await controller.Sort(ProductSortOrder.Recommended.ToString());

            //Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var sortedProducts = okObjectResult.Value as List<Product>;
            Assert.NotNull(sortedProducts);
            for (int i = 0; i < sortedProducts.Count; i++)
            {
                Assert.True(sortedProducts[i].Name == expectedOrderedList[i]);
            }
        }

        [Fact]
        public async void CheckProductSortOptionByJunkValueSortToAscendingSuccess()
        {
            //Arrange
            var controller = MockProductControllerSetup();
            var expectedOrderedList = new List<string> { "A", "B", "C", "D", "F" };

            //Act
            var actionResult = await controller.Sort("JunkValue");

            //Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var sortedProducts = okObjectResult.Value as List<Product>;
            Assert.NotNull(sortedProducts);
            for (int i = 0; i < sortedProducts.Count; i++)
            {
                Assert.True(sortedProducts[i].Name == expectedOrderedList[i]);
            }
        }
        private ProductController MockProductControllerSetup()
        {
            var logger = Mock.Of<ILogger<ProductController>>();
            var loggerProductService = Mock.Of<ILogger<ProductSortService>>();
            var wooliesXHttpClientMock = new Mock<IWooliesXHttpClient>();
            wooliesXHttpClientMock.Setup(m => m.GetProductListAsync()).Returns(async () => RandomProductList());
            wooliesXHttpClientMock.Setup(m => m.GetShopperHistoryProductListAsync())
                .Returns(async () => RandomCustomerShopperHistory());
            var productSortService = new ProductSortService(loggerProductService);
            var controller = new ProductController(wooliesXHttpClientMock.Object, productSortService, logger);
            return controller;
        }

        private List<Product> RandomProductList()
        {
            var products = new List<Product>
            {
                new Product {Name = "A", Price = 6, Quantity = 0},
                new Product {Name = "F", Price = 4, Quantity = 0},
                new Product {Name = "B", Price = 3, Quantity = 0},
                new Product {Name = "C", Price = 2, Quantity = 0},
                new Product {Name = "D", Price = 5, Quantity = 0}
            };
            return products;
        }
        private List<Product> RandomProductList1()
        {
            var products = new List<Product>
            {
                new Product {Name = "A", Price = 6, Quantity = 2},
                new Product {Name = "F", Price = 4, Quantity = 1},
                new Product {Name = "B", Price = 3, Quantity = 3},
                new Product {Name = "C", Price = 2, Quantity = 4},
                new Product {Name = "D", Price = 5, Quantity = 1}
            };
            return products;
        }

       
        private List<Product> RandomProductList2()
        {
            var products = new List<Product>
            {
                new Product {Name = "C", Price = 6, Quantity = 2},
                new Product {Name = "R", Price = 4, Quantity = 4},
                new Product {Name = "B", Price = 3, Quantity = 3},
                new Product {Name = "C", Price = 2, Quantity = 5},
                new Product {Name = "D", Price = 5, Quantity = 1}
            };
            return products;
        }

        private List<ShopperHistory> RandomCustomerShopperHistory()
        {
            var shopperHistoryList = new List<ShopperHistory>()
            {

                new ShopperHistory()
                {
                    CustomerId = 1,
                    Products = RandomProductList1()
                },
                new ShopperHistory()
                {
                    CustomerId = 2,
                    Products = RandomProductList2()
                }
            };

            return shopperHistoryList;
        }
    }
}
