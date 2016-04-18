using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
   public class Datadictionary
    {
        [JsonProperty("code")]
       public string Code { get;set;}

        [JsonProperty("text")]
       public string Text { get; set; }

    }
}
