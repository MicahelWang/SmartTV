using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Enum;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class DeviceTraceCriteria : BaseSearchCriteria
    {
        public string DeviceSeries { get; set; }
        public string Platfrom { get; set; }
        public string Token { get; set; }
        public string HotelId { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string RoomNo { get; set; }
        public DeviceType? DeviceType { get; set; }

        public bool? Active { get; set; }
    }
}
