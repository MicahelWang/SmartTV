using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YeahTVApi.DomainModel.Models
{
    public partial class TvTemplateType : BaseEntity<int>
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTvTemplateTypeName")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_TvTemplateType_Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTvTemplateTypeDescription")]
        public string Description { get; set; }
        [JsonIgnore]
        public virtual ICollection<TvTemplate> Templates { get; set; }
    }
}
