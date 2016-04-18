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
    public partial class CoreSysGroup : BaseEntity<string>
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSysGroupCode")]
        public string GroupCode { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSysGroupName")]
        public string GroupName { get; set; }
        public string ApiKey { get; set; }
        public Nullable<int> TemplateId { get; set; }
         [JsonIgnore]
         [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayActive")]
        public bool IsDelete { get; set; }
        [JsonIgnore]
        public ICollection<CoreSysBrand> CoreSysBrands { get; set; }

        //[JsonIgnore]
        //public ICollection<ErpSysUser> ErpSysUser { get; set; }

    }
}
