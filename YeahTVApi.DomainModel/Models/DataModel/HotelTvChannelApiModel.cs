using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public class HotelTVChannelApiModel
    {
        public string AdImgUrl { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string Icon { get; set; }
        public string Category { get; set; }
        public string CategoryEn { get; set; }
        public string HotelId { get; set; }
        public string ChannelId { get; set; }
        public string ChannelCode { get; set; }
        public string HostAddress { get; set; }
        public int ChannelOrder { get; set; }
    }
}
