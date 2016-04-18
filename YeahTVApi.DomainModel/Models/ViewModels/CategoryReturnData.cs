using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    public class CategoryReturnData
    {
        [JsonProperty("sentRoom")]
        public bool SentRoom { get; set; }
        [JsonProperty("categorys")]
        public List<CommdityCategory> Categorys { get; set; }
    }
}
