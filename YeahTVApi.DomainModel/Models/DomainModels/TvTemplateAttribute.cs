using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models
{
    public partial class TvTemplateAttribute : BaseEntity<string>
    {
         [JsonProperty("attributeText")]
        public string Text { get; set; }
         [JsonProperty("attributeValue")]
        public string Value { get; set; }
        public string ElementId { get; set; }
        public bool Required { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTvTemplateAttributeEditable")]
        public bool Editable { get; set; }
        [JsonProperty("attributeParentId")]
        public string ParentId { get; set; }
        [JsonProperty("attributeDataType")]
        public int DataType { get; set; }
         [JsonProperty("attributeDescription")]
        public string Description { get; set; }

        [JsonIgnore]
        public virtual TvTemplateElement Element { get; set; }
        [JsonIgnore]
        public virtual ICollection<TvDocumentAttribute> DocumentAttributes { get; set; } 
    }
}
