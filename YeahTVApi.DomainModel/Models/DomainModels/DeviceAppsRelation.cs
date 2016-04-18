using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public class DeviceAppsRelation
    {
        public string DeviceSeries { get; set; }

        public string AppId { get; set; }

        public string Action { get; set; }
    }
}
