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
    public partial class ErpPowerRole : BaseEntity<string>
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayErpRoleCode")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_ErpPowerRole_RoleCode")]
        public string RoleCode { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayErpRoleName")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_ErpPowerRole_RoleName")]
        public string RoleName { get; set; }


        [JsonIgnore]
        public ICollection<ErpSysUser> ErpSysUser { get; set; }
    }
}
