using Newtonsoft.Json;
using System.Collections.Generic;

namespace WooliesXAPI.DataContracts
{
    public class GetTrolleyTotalRequest
    {
        [JsonProperty("products")] public List<ProductPrice> Products { get; set; }

        [JsonProperty("specials")] public List<Special> Specials { get; set; }

        [JsonProperty("quantities")] public List<Quantity> Quantities { get; set; }
    }
    public class ProductPrice
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("price")] public decimal Price { get; set; }
    }
    public class Quantity
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("quantity")] public long QuantityQuantity { get; set; }
    }

    public class Special
    {
        [JsonProperty("quantities")] public List<Quantity> Quantities { get; set; }

        [JsonProperty("total")] public decimal Total { get; set; }
    }
}
