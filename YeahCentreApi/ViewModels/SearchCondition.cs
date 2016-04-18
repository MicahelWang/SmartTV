using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YeahTVApi.DomainModel.Models;

namespace YeahCentreApi.ViewModels
{
    public class SearchCondition
    {
        [JsonProperty("pagesize")]
        public int Pagesize { get; set; }
        [JsonProperty("status",NullValueHandling=NullValueHandling.Ignore )]
        public string Status { get; set; }


        [JsonProperty("hotelid")]
        public string HotelId { get; set; }

        [JsonProperty("pageindex")]
        public int Pageindex { get; set; }

        [JsonProperty("enddate")]
        [JsonConverter(typeof(StringDateTimeConverter))]
        public DateTime Enddate { get; set; }

        [JsonProperty("begindate")]
        [JsonConverter(typeof(StringDateTimeConverter))]
        public DateTime Begindate { get; set; }

        [JsonProperty("roomnumber")]
        public string Roomnumber { get; set; }

     


    }
}