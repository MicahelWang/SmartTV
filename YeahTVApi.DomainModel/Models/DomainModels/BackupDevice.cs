using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public partial class BackupDevice : BaseEntity<int>
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayDeviceSeries")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_BackupDevice_DeviceSeries")]
        public string DeviceSeries { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayHotelId")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_BackupDevice_HotelId")]
        public string HotelId { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayLastUpdateTime")]
        public System.DateTime LastUpdateTime { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayLastUpdatUser")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_BackupDevice_LastUpdatUser")]
        public string LastUpdatUser { get; set; }

        public string Model { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayActive")]
        public bool Active { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayDeviceType")]
        public string DeviceType { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayIsUsed")]
        public bool IsUsed { get; set; }
         
    }
}
