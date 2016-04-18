using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YeahTVApi.DomainModel.Models
{
    public partial class TvDocumentElement : BaseEntity<string>
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTvDocumentElementName")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTvDocumentElementParentId")]
        public string ParentId { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTvDocumentElementTemplateId")]
        public string TemplateId { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTvDocumentElementOrders")]
        public int Orders { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTvDocumentElementTemplateElementId")]
        public string TemplateElementId { get; set; }
        public virtual ICollection<TvDocumentAttribute> Attributes { get; set; }      
        public virtual TvTemplateElement TemplateElement { get; set; }
    }
}
