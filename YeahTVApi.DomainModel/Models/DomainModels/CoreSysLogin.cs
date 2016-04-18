using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YeahTVApi.DomainModel.Models
{
    public partial class CoreSysLogin : BaseEntity<string>
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSysLoginName")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_CoreSysLogin_LoginName")]
        public string LoginName { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSysUserName")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSysPassword")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_CoreSysLogin_Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public Nullable<DateTime> RegDate { get; set; }
        public Nullable<DateTime> LastLoginDate { get; set; }
        public string AddUserId { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSysState")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_CoreSysLogin_State")]
        public int State { get; set; }

        public bool IsDelete { get; set; }


        public virtual ErpSysUser ErpSysUser { get; set; }
    }
}
