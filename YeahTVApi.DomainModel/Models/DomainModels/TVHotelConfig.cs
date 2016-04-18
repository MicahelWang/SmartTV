using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YeahTVApi.DomainModel.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("TvHotelconfig")]
    public partial class TVHotelConfig : BaseEntity<int>
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTVHotelConfigHotelId")]
        [Required(ErrorMessageResourceName = "Common_ErrorMessageForTVHotelConfigHotelId", ErrorMessageResourceType = typeof(Resource.Resource))]
        public string HotelId { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTVHotelConfigConfigCode")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_TVHotelConfig_ConfigCode")]
        [MaxLength(32)]
        public string ConfigCode { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTVHotelConfigConfigName")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_TVHotelConfig_ConfigName")]
        [MaxLength(32)]
        public string ConfigName { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTVHotelConfigConfigValue")]
        //[Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_TVHotelConfig_ConfigValue")]   2015-06-09 ¸ù¾ÝStevenËµ
        [MaxLength(500)]
        //[RegularExpression("^[0-9]*$",ErrorMessage=" ")]
        public string ConfigValue { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayActive")]

        public Nullable<bool> Active { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTVHotelConfigLastUpdater")]
        public string LastUpdater { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTVHotelConfigLastUpdateTime")]
        public Nullable<System.DateTime> LastUpdateTime { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTVHotelConfigCreateTime")]
        public Nullable<System.DateTime> CreateTime { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public virtual YeahTVApi.DomainModel.Models.ViewModels.StorePayConfig StorePayConfig { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public bool IsEnableMeng { get; set; }
    }

    public partial class TVHotelConfigForApi
    {
        public string HotelId { get; set; }
        public string ConfigCode { get; set; }
        public string ConfigName { get; set; }
        public string ConfigValue { get; set; }
    }
}
