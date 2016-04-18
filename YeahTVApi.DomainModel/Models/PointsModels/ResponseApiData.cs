using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models.DomainModels;

namespace YeahTVApi.DomainModel.Models.PointsModels
{
    public class ResponseApiData<T> : BaseReturnMessage
    {
        [JsonProperty("data",Order=2)]
        public T Data { get; set; }
    }
}
