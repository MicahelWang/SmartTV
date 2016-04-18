using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace YeahTVIntegralExchange.Models
{
    public class ResponseApiData<T> : BaseReturnMessage
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }
    public class PostApiParameters<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
