using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    [Serializable]
    public class DocumentElementDto : ElementDto
    {
        public bool isallowchild { get; set; }
        public bool editable { get; set; }
    }
}
