using System.ComponentModel.DataAnnotations;
using YeahTVApi.DomainModel.Resource;

namespace YeahAppCentre.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required_CoreSysLogin_LoginName")]
        [MinLength(6, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required_TvTemplateType_MinLoginNameLength")]
        [MaxLength(16, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required_TvTemplateType_MaxLoginNameLength")]
        [Display(ResourceType = typeof(Resource), Name = "Common_DisplayCoreSysUserName")]
        public string LoginName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required_CoreSysLogin_Password")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required_RegisterViewModel_lessChart", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resource), Name = "Common_DisplayCoreSysPassword")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resource), Name = "Common_DisplayCoreSysConfirmPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required_RegisterViewModel_passwordCompare")]
        public string ConfirmPassword { get; set; }
    }
}