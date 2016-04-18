using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.DataModel
{
   public class ReqestCommodityCate
    {
        [JsonProperty("hotelId")]
       public string HotelId { get; set; }
    }
}
