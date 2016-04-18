using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models
{
    public partial class DeviceAppsMonitor : BaseEntity<string>
    {
        public DeviceAppsMonitor()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string DeviceSeries { get; set; }
        public string Action { get; set; }
        public string PackageName { get; set; }
        public int VersionCode { get; set; }
        public string VersionName { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public bool Active { get; set; }
    }
}
