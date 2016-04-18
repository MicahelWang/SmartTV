using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YeahTVApi.DomainModel.Models
{
    public partial class TvTemplateElement : BaseEntity<string>
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTvTemplateElementName")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTemplateType")]
        public int TemplateType { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayParentId")]
        public string ParentId { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayOrders")]
        public int Orders { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayEditable")]
        public bool Editable { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayIsChildFrame")]
        public bool IsChildFrame { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayIsAllowChild")]
        public bool IsAllowChild { get; set; }


        public virtual ICollection<TvTemplateAttribute> Attributes { get; set; }

        [JsonIgnore]
        public virtual ICollection<TvDocumentElement> DocumentElements { get; set; } 
    }
}
