using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Enum;

namespace YeahTVApi.DomainModel.Models
{
    public partial class ErpSysUser : BaseEntity<string>
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayErpGroupId")]
        public string GroupId { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayErpHotelId")]
        public string HotelId { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayErpUserName")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_ErpSysUser_UserName")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayErpUserType")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_ErpSysUser_UserType")]
        [EnumDataType(typeof(UserTypeEnum))]
        public int? UserType { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayErpPhone")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayErpEmail")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_ErpSysUser_Email")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "Common_ErrorMessageForErpEmail", ErrorMessageResourceType = typeof(Resource.Resource))]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayErpRoleId")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_ErpSysUser_RoleId")]
        public string RoleId { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayErpCreateDate")]
        public DateTime? CreateDate { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayErpModifyDate")]
        public System.DateTime ModifyDate { get; set; }

        public string CreateUser { get; set; }
        public string ModifyUser { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayErpState")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_ErpSysUser_State")]
        public int State { get; set; }

        public bool IsDelete { get; set; }

        // public virtual CoreSysGroup CoreSysGroup { get; set; }
        public virtual ErpPowerRole ErpPowerRole { get; set; }
        // public virtual CoreSysHotel CoreSysHotel { get; set; }

        public virtual CoreSysLogin CoreSysLogin { get; set; }
    }
}
