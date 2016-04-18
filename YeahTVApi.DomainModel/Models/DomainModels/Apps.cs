using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models
{
    public partial class Apps : BaseEntity<string>
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayAppsName")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_Apps_Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayPlatfrom")]
        public string Platfrom { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayDescription")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayPackageName")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_Apps_PackageName")]
        public string PackageName { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayAppKey")]
        public string AppKey { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayActive")]
        public bool Active { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayLastUpdater")]
        public string LastUpdater { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayIconUrl")]
        public string IconUrl { get; set; }

        public Nullable<bool> ShowInStroe { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayLastUpdateTime")]
        public Nullable<System.DateTime> LastUpdateTime { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCreateTime")]
        public System.DateTime CreateTime { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplaySecureKey")]
        public string SecureKey { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayIsSystem")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_Apps_IsSystem")]
        public bool IsSystem { get; set; }

        [JsonIgnore]
        public virtual ICollection<AppVersion> AppVresions { get; set; }
    }
}
