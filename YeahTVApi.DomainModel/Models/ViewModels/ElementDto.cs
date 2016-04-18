using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    [Serializable]
    public class ElementDto
    {
        public string id { get; set; }
        public string pId { get; set; }
        public string name { get; set; }
        public bool ischecked { get; set; }
        public bool open { get; set; }
        public string icon { get; set; }
        public bool drag { get; set; }
        public List<AttributeDto> Attributes { get; set; } 
    }

    public class AttributeDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
