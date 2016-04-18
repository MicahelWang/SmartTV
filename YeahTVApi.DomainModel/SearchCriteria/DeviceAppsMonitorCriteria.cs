using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class DeviceAppsMonitorCriteria : BaseSearchCriteria
    {
        public string DeviceSeries { get; set; }
        public string Action { get; set; }
        public string PackageName { get; set; }
        public string VersionCode { get; set; }
        public Nullable<DateTime> UpdateTime { get; set; }
        public bool? Active { get; set; }
    }
}
