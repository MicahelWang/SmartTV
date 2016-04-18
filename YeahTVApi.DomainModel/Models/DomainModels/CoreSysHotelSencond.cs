using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YeahTVApi.DomainModel.Models
{
    public partial class CoreSysHotelSencond : BaseEntity<string>
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSysAutoToHome")]
        public bool AutoToHome { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSysLanguages")]
        [Required(ErrorMessageResourceName = "Required_CoreSysHotelSencond_Languages", ErrorMessageResourceType = typeof(Resource.Resource))]
        public int Languages { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSysWelcomeWord")]
        [StringLength(200)]
        
        public string WelcomeWord { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSysLaunchBackground")]
        [StringLength(200)]
       
        public string LaunchBackground { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSysLocalPMSUrl")]
        [StringLength(200)]
        public string LocalPMSUrl { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSysPriceOfDay")]
       
        public decimal PriceOfDay { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSysAdUrl")]
        [StringLength(200)]
        public string AdUrl { get; set; }


        public string BaseData { get; set; }

        [JsonIgnore]
        public virtual CoreSysHotel CoreSysHotel { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSysLogoImageUrl")]
        //[Required(ErrorMessageResourceName = "Required_CoreSysHotelSencond_LogoImageUrl", ErrorMessageResourceType = typeof(Resource.Resource))]
        public string LogoImageUrl { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCoreSys_VodAddress")]
        public string VodAddress { get; set; }
       
    }
}
