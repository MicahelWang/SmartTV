using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models.DataModel
{
    public class HotelChannelUsedTime
    {
        public string HotelId { get; set; }
        public string ChannelId { get; set; }
        public double UsedTime { get; set; }
        public DateTime Date { get; set; }
    }
    public class ChannelUsedItem
    {
        [JsonProperty("channelid")]
        public string ChannelId { get; set; }
        [JsonProperty("usedtime")]
        public double UsedTime { get; set; } 
    }

}
