using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models
{
    public class HCSGlobalConfig
    {
        [JsonProperty("config_no")]
        public string ConfigNo { get; set; }

        [JsonProperty("refresh_config_frequency")]
        public string RefreshConfigFrequency { get; set; }

        [JsonProperty("task_scan_frequency")]
        public string TaskScanFrequency { get; set; }

        [JsonProperty("auto_speed_frequency")]
        public string AutoSpeedFrequency { get; set; }

        [JsonProperty("test_file_url")]
        public string TestFileUrl { get; set; }

        [JsonProperty("monitor_send_frequency")]
        public string MonitorSendFrequency { get; set; }

        [JsonProperty("cpu_scan_frequency")]
        public string CpuScanFrequency { get; set; }

        [JsonProperty("ram_scan_frequency")]
        public string RamScanFrequency { get; set; }

        [JsonProperty("disk_scan_frequency")]
        public string DiskScanFrequency { get; set; }
    }
}
