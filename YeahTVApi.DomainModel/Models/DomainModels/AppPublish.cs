using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YeahTVApi.DomainModel.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace YeahTVApi.DomainModel.Models
{
    public partial class AppPublish : BaseEntity<string>
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayPublishDate")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_AppPublish_PublishDate")]
        public DateTime PublishDate { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayActive")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_AppPublish_Active")]
        public bool Active { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayVersionCode")]
        [Required(ErrorMessageResourceType=typeof(Resource.Resource),ErrorMessageResourceName="Required_AppPublish_VersionCode")]
        public int VersionCode { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayLastUpdater")]
        public string LastUpdater { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayErpModifyDate")]
        public Nullable<System.DateTime> LastUpdateTime { get; set; }
        public System.DateTime CreateTime { get; set; }
        public virtual  AppVersion AppVersion { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_AppPublish_HotelName")]
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayHotelName")]
        public string HotelId { get; set; }
    }
}
