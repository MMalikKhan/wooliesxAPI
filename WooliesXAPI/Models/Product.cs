using Newtonsoft.Json;

namespace WooliesXAPI.Models
{
    public partial class Product
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("price")] public double Price { get; set; }

        [JsonProperty("quantity")] public long Quantity { get; set; }
    }
}
