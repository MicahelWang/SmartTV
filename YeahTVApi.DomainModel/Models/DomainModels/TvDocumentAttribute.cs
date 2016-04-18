using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace YeahTVApi.DomainModel.Models
{
    public partial class TvDocumentAttribute : BaseEntity<string>
    {
        public string Value { get; set; }
        public string ElementId { get; set; }
        public string Text { get; set; }
        public string TemplateAttributeId { get; set; }
        public string ParentId { get; set; }
        [JsonIgnore]
        public virtual TvDocumentElement Element { get; set; }

        public virtual TvTemplateAttribute TemplateAttribute { get; set; }
    }
}
