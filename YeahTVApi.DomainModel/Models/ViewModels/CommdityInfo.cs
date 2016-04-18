using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    public class CommdityInfo
    {
        [JsonProperty("categoryId")]
        public string CategoryId { get; set; }
        [JsonProperty("pageIndex")]
        public int Pageindex { get; set; }
        [JsonProperty("pageSize")]
        public int Pagesize { get; set; }

    }
}
