using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
   public class GoodIdArray
    {
        [JsonProperty("goodsIds")]
       public List<string> GoodsIds { get; set; }
    }
}
