using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models
{
    public class PromulgateTokenResult
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("orderid")]
        public string OrderId { get; set; }
    }
}