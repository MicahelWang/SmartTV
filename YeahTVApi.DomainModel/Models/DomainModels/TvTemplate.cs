using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YeahTVApi.DomainModel.Models
{
    public partial class TvTemplate : BaseEntity<string>
    {
        public TvTemplate()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTvTemplateName")]
        public string Name { get; set; }

        public int TemplateTypeId { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTvTemplateDescription")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTvTemplateCreateUser")]
        public string CreateUser { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTvTemplateCreateDate")]
        public System.DateTime CreateDate { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTvTemplateModifyUser")]
        public string ModifyUser { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTvTemplateModifyDate")]
        public System.DateTime ModifyDate { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTvTemplateTemplateType")]
        public virtual TvTemplateType TemplateType { get; set; }

        [JsonIgnore]
        public virtual ICollection<CoreSysHotel> CoreSysHotel { get; set; }

    }
}
