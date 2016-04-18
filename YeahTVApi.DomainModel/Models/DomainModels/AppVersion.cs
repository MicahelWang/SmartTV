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
    public partial class AppVersion : BaseEntity<string>
    {
        //public AppVersion()
        //{

        //    Active = true;
            
        //}
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayVersionCode")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_AppVersion_VersionCode")]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "Common_RangeVersionCode", ErrorMessageResourceType = typeof(Resource.Resource))]
        public int VersionCode { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayVersionName")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_AppVersion_VersionName")]
        public string VersionName { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayActive")]
        public bool Active { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayLastUpdater")]
        public string LastUpdater { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayLastUpdateTime")]
        public DateTime LastUpdateTime { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCreateTime")]
        public DateTime CreateTime { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayAppUrl")]
        public string AppUrl { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayAppVersionDescription")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayFileName")]
        public string FileName { get;set;}

        public virtual Apps App { get; set; }

        [JsonIgnore]
        public virtual ICollection<AppPublish> AppPublishs { get; set; }
        [NotMapped]
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_IsLocalFileAddress")]
        public bool IsLocalFileAddress { get; set; }      
    }
}
