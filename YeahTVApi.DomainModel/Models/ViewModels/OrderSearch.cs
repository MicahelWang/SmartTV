using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
  public  class OrderSearch
    {
        [JsonProperty("pagetotal")]

        public int PageTotal { get; set; }
      
        [JsonProperty("pageindex")]
        public int Pageindex { get; set; }

        [JsonProperty("pagesize")]
        public int Pagesize { get; set; }

        public List<StoreOrder> Storeorders { get; set; }


    }
}
