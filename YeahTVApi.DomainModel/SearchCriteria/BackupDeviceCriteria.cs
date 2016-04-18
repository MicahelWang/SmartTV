using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class BackupDeviceCriteria : BaseSearchCriteria
    {
        public string DeviceSeries { get; set; }
        public string HotelId { get; set; }
        public string LastUpdatUser { get; set; }
        public bool? Active { get; set; }
        public bool IsUsed { get; set; }
    }
}
