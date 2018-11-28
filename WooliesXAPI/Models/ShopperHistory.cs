using Newtonsoft.Json;
using System.Collections.Generic;

namespace WooliesXAPI.Models
{
    public class ShopperHistory
    {
        [JsonProperty("customerId")]
        public int CustomerId { get; set; }
        [JsonProperty("products")]
        public List<Product> Products { get; set; }
    }
}
