using Newtonsoft.Json;

namespace WooliesXAPI.DataContracts
{
    public class GetUserResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
