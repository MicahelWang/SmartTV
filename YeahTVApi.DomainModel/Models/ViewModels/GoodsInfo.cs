using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
   public class GoodsInfoList
    {
        [JsonProperty("pageTotal")]
       public int PageTotal { get; set; }
        [JsonProperty("goods")]
       public List<GoodsInfomation> Goods { get; set; }
    }
  
}
