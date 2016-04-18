using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class HotelTVChannelCriteria : BaseSearchCriteria
    {
        public string HotelId { get; set; }
        public string ChannelId { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string Icon { get; set; }
        public string Category { get; set; }
        public string CategoryEn { get; set; }
        public string ChannelCode { get; set; }
        public string HostAddress { get; set; }
        public string ChannelOrder { get; set; }
    }
}
