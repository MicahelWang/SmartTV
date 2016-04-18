using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YeahTVApi.DomainModel.Enum;

namespace YeahCentreApi.ViewModels
{
    public class DictRequest
    {
        [JsonProperty("hotelid")]
        public string HotelId { get; set;}

        [JsonProperty("datatype")]
        public string DataType { get; set; }
    }
}