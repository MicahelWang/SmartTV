using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YeahTVApi.DomainModel.Models
{
    public partial class CoreSysHotel : BaseEntity<string>
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayHotelCode")]
        [Required(ErrorMessageResourceName = "Common_ErrorMessageForHotelCode", ErrorMessageResourceType = typeof(Resource.Resource))]
        [StringLength(20)]
        public string HotelCode { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayHotelName")]
        [Required(ErrorMessageResourceName = "Common_ErrorMessageForHotelName", ErrorMessageResourceType = typeof(Resource.Resource))]
        [StringLength(20)]
        public string HotelName { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayHotelNameEn")]
        [StringLength(50)]
        public string HotelNameEn { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayGroupId")]
        [StringLength(50)]
        public string GroupId { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayBrand")]
        [Required(ErrorMessageResourceName = "Common_ErrorMessageForBrandId", ErrorMessageResourceType = typeof(Resource.Resource))]
        [StringLength(50)]
        public string BrandId { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayIsLocalPMS")]
        public bool IsLocalPMS { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTel")]
        [StringLength(20)]
        public string Tel { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayProvince")]
        public int Province { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCity")]
        public int City { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayCountry")]
        [Range(0, Int32.MaxValue)]
        [Required(ErrorMessageResourceName = "Common_ErrorMessageForCountry", ErrorMessageResourceType = typeof(Resource.Resource))]
        public int Country { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayAddress")]
        [StringLength(255)]
        public string Address { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayLongitude")]
        public decimal Longitude { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayLatitude")]
        public decimal Latitude { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTemplateId")]
        [StringLength(50)]
        public string TemplateId { get; set; }

        [JsonIgnore]
        public bool IsDelete { get; set; }

        public virtual CoreSysBrand CoreSysBrand { get; set; }
        public virtual CoreSysHotelSencond CoreSysHotelSencond { get; set; }
        public virtual TvTemplate TvTemplate { get; set; }
        public DateTime CreateTime { get; set; }
        //[JsonIgnore]
        //public ICollection<ErpSysUser> ErpSysUser { get; set; }
    }
}
