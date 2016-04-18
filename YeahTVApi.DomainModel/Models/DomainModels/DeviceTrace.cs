using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
namespace YeahTVApi.DomainModel.Models
{
    public partial class DeviceTrace : BaseEntity<string>
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayDeviceSeries")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_DeviceTrace_DeviceSeries")]
        public string DeviceSeries { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayFirstVisitTime")]
        public System.DateTime FirstVisitTime { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayLastVisitTime")]
        public Nullable<System.DateTime> LastVisitTime { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayDeviceKey")]
        public string DeviceKey { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayIp")]
        public string Ip { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayPlatfrom")]
        public string Platfrom { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayBrand")]
        public string Brand { get; set; }

        public string Manufacturer { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayModel")]
        public string Model { get; set; }

        public string OsVersion { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayHotelId")]  
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_DeviceTrace_HotelId")]
        public string HotelId { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayRoomNo")]
        public string RoomNo { get; set; }

        public Nullable<int> ModelId { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayActive")]
        public bool Active { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayRemark")]
        public string Remark { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayDeviceType")]
        public string DeviceType { get; set; }

        public string Token { get; set; }
        public string GuestId { get; set; }
        public string GroupId { get; set; }

        public string Attachments { get; set; }
        [NotMapped]
        public List<string> ListAttachments { get; set; }
    }

    public partial class  SimpDeviceTrace : BaseEntity<string>
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayDeviceSeries")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_DeviceTrace_DeviceSeries")]
        public string DeviceSeries { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayAppVersion")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_DeviceTrace_AppVersion")]
        public string AppVersion { get; set; }

      
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayLastVisitTime")]
        public Nullable<System.DateTime> LastVisitTime { get; set; }

      

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayIp")]
        public string Ip { get; set; }

       
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayBrand")]
        public string Brand { get; set; }

        public string Manufacturer { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayModel")]
        public string Model { get; set; }

        public string OsVersion { get; set; }

       

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayHotelId")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_DeviceTrace_HotelId")]
        public string HotelId { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayRoomNo")]
        public string RoomNo { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayActive")]
        public bool Active { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayRemark")]
        public string Remark { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayDeviceType")]
        public string DeviceType { get; set; }
        public string Attachments { get; set; }
        public List<string> listAttachments { get; set; }
    }
}
