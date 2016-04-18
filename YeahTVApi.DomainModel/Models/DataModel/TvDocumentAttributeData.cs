using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Enum;
using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models.DataModel
{
    public class TvDocumentAttributeData
    {
         [JsonProperty("attributeId")]
        public string AttributeId { get; set; }
         [JsonProperty("attributeText")]
        public string AttributeText { get; set; }
         [JsonProperty("attributeParentId")]
        public string AttributeParentId { get; set; }
         [JsonProperty("attributeValue")]
        public string AttributeValue { get; set; }
         [JsonProperty("attributeDescription")]
        public string AttributeDescription { get; set; }
         [JsonProperty("attributeDataType")]
        public TemplateDataType AttributeDataType { get; set; }


    }
}
