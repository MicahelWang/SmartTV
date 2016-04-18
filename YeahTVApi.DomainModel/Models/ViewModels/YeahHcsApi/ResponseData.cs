using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models.YeahHcsApi
{
    public class ResponseData<T>
    {
        [JsonProperty("sign",Order=0)]
        public string Sign { get; set; }

        [JsonProperty("data",Order=1)]
        public T Data { get; set; }
    }
}