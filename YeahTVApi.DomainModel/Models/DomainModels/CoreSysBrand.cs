using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YeahTVApi.DomainModel.Models
{
    public partial class CoreSysBrand : BaseEntity<string>
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSysBrandName")]
        [Required(ErrorMessageResourceName = "Required_Brand_BrandName", ErrorMessageResourceType = typeof(Resource.Resource))]
        public string BrandName { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSysBrandCode")]
        public string BrandCode { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSysGroupName")]
        [Required(ErrorMessageResourceName = "Required_Brand_GroupId", ErrorMessageResourceType = typeof(Resource.Resource))]
        public string GroupId { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSysTemplateId")]
        public Nullable<int> TemplateId { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSysLogo")]
        public string Logo { get; set; }

         [JsonIgnore]
        public bool IsDelete { get; set; }
        public virtual CoreSysGroup CoreSysGroup { get; set; }
        [JsonIgnore]
        public ICollection<CoreSysHotel> CoreSysHotels { get; set; } 
    }
}
