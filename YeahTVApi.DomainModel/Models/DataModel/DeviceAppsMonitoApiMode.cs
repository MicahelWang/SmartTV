using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.DataModel
{
    public class DeviceAppsMonitoApiMode
    {
        public string DeviceSeries { get; set; }
        public string Action { get; set; }
        public string PackageName { get; set; }
        public int VersionCode { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public bool Active { get; set; }
        public string AppUrl { get; set; }
    }
}
