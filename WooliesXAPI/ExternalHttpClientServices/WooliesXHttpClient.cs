using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WooliesXAPI.Common;
using WooliesXAPI.DataContracts;
using WooliesXAPI.Models;
using Product = WooliesXAPI.Models.Product;

namespace WooliesXAPI.ExternalHttpClientServices
{
    public interface IWooliesXHttpClient
    {
        Task<string> FindLowestTrolleyCalculator(GetTrolleyTotalRequest request);
        Task<List<Product>> GetProductListAsync();
        Task<List<ShopperHistory>> GetShopperHistoryProductListAsync();
    }

    public class APIConstants
    {
        public const string GetTrolleyCalculatorResourceAPI = "api/resource/trolleyCalculator?token={0}";
        public const string ProductListAPI = "api/resource/products?token={0}";
        public const string ShoppingHistoryAPI = "api/resource/shopperHistory?token={0}";
    }

    public class WooliesXHttpClient : IWooliesXHttpClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<WooliesXHttpClient> _logger;

        public WooliesXHttpClient(HttpClient httpClient, ILogger<WooliesXHttpClient> logger)
        {
            httpClient.BaseAddress = new Uri("http://dev-wooliesx-recruitment.azurewebsites.net/");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "MalikKhan-WooliesXAPI");
            _client = httpClient;
            _logger = logger;
        }

        public async Task<string> FindLowestTrolleyCalculator(GetTrolleyTotalRequest request)
        {
            _logger.LogTrace("retreiving GetTrolleyCalculator list");
            var content = JsonConvert.SerializeObject(request);
            _logger.LogTrace($"FindLowestTrolleycalculator content {content}");
            return await RequestHttpClient<string>(HttpMethod.Post,
                string.Format(APIConstants.GetTrolleyCalculatorResourceAPI, ApplicationConstants.UserToken),
                JsonConvert.SerializeObject(request));
        }

        public async Task<List<Product>> GetProductListAsync()
        {
            _logger.LogTrace("retreiving products list");
            return await RequestHttpClient<List<Product>>(HttpMethod.Get,
                string.Format(APIConstants.ProductListAPI, ApplicationConstants.UserToken));

        }

        public async Task<List<ShopperHistory>> GetShopperHistoryProductListAsync()
        {
            _logger.LogTrace("retreiving shoppers history");
            return await RequestHttpClient<List<ShopperHistory>>(HttpMethod.Get,
                string.Format(APIConstants.ShoppingHistoryAPI, ApplicationConstants.UserToken));

        }

        private async Task<T> RequestHttpClient<T>(HttpMethod httpMethod, string relativeUrl, string jsonContent = "")
        {
            try
            {
                using (var request = new HttpRequestMessage(httpMethod, relativeUrl))
                {
                    using (var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
                    {
                        request.Content = stringContent;

                        using (var response = await _client
                            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                            .ConfigureAwait(false))
                        {
                            response.EnsureSuccessStatusCode();
                            var responseString = await response.Content.ReadAsStringAsync();
                            return JsonConvert.DeserializeObject<T>(responseString);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in RequestHttpClient url {relativeUrl} jsonContent {jsonContent}");
                throw;
            }
        }
    }
}
