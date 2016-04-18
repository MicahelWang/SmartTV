using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    public class CommdityCategory
    {
        [JsonProperty("id")]
        public string CategoryId
        {
            get;
            set;
        }
        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }
        [JsonProperty("sort")]
        public string Index
        {
            get;
            set;
        }
        [JsonProperty("selectImage")]
        public string SelectImage
        {
            get;
            set;
        }
        [JsonProperty("unselectImage")]
        public string UnselectImage
        {
            get;
            set;
        }
    }
}
